using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Linq;
using System.Reflection;
using System.Linq.Expressions;
using MongoRepository.Pagination;
using ESB.Utils.Serializers;
using System.Linq;

namespace MongoRepository
{
    public class MRepository<T> where T : class

    {
        private readonly BaseRepository<T> contexto;

        public MRepository()
        {
            contexto = new BaseRepository<T>();
        }

        public void Save(T entity)
        {
            contexto.Collection.Save(entity);
        }

        public void Remove(string id)
        {
            contexto.Collection.Remove(MongoDB.Driver.Builders.Query.EQ("_id", id));
        }

        public T Get(string id)
        {
            return contexto.Collection.FindOneByIdAs<T>(id);
        }

        public List<T> GetAll()
        {
            return contexto.Collection.AsQueryable().ToList();
        }

        public List<T> GetByFilter(Func<T, bool> filter)
        {
            return contexto.Collection.AsQueryable().Where(filter).ToList(); 
        }

        public List<T> Find(MongoDB.Driver.IMongoQuery queryString)
        {
            return contexto.Collection.Find(queryString).ToList(); 
        }

         Dictionary<string, Func<T, object>> compiledProperties;
        public  Dictionary<string, Func<T, object>> CompiledProperties
        {
            get
            {
                if (compiledProperties == null)
                {
                    compiledProperties = new Dictionary<string, Func<T, object>>();
                }
                return compiledProperties;
            }
        }

         Dictionary<string, PropertyInfo> properties;
        public  Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new Dictionary<string, PropertyInfo>();
                }
                return properties;
            }
        }

        public PropertyInfo GetPropertyByName(string propName)
        {
            if (Properties.ContainsKey(propName))
                return properties[propName];

            Type personType = typeof(T);

            if (personType.GetProperties().Any(prop => prop.Name == propName && prop.CanRead))
            {
                PropertyInfo pinfo = personType.GetProperty(propName);
                properties[propName] = pinfo;

                return pinfo;
            }

            return null;
        }

        public Func<T, object> GetOrderFunction(string propName)
        {
            if (CompiledProperties.ContainsKey(propName))
                return compiledProperties[propName];

            Type personType = typeof(T);

            PropertyInfo pinfo = GetPropertyByName(propName);
            if (pinfo == null)
                return null;

            ParameterExpression paramExpr = Expression.Parameter(typeof(T), "instance");
            MemberExpression memberExpr = Expression.Property(paramExpr, pinfo);

            Func<T, object> orderByFunc = Expression.Lambda<Func<T, object>>(memberExpr, paramExpr).Compile();

            CompiledProperties[propName] = orderByFunc;

            return orderByFunc;
        }
        protected virtual Expression<Func<T, bool>> GetFuncExpr(FilterCriteria ft)
        {
            return null;
        }
        public PageResult<T> GetPage(string request)
        {
            PageRequest pr = request.JsonDeserialize<PageRequest>();

            PageResult<T> result = new PageResult<T>();
            result.PageRequest = pr;


            pr.PageIndex = pr.PageIndex - 1;
            var cursor = contexto.Collection.AsQueryable();

            //IEnumerable<T> list = null;
            if (pr == null)
                return null;

            
            if (pr.FilterCriteria != null && pr.FilterCriteria.Count > 0)
            {
                for (int i = 0; i < pr.FilterCriteria.Count; i++)
                {
                    PropertyInfo pInfo = GetPropertyByName(pr.FilterCriteria[i].FieldName);
                    pr.FilterCriteria[i].Index = i;
                    pr.FilterCriteria[i].ValueType = pInfo.PropertyType;

                    //string expression = pr.FilterCriteria[i].Expression;

                    //expression = "UserGuid == @0";
                    ////UserGuid.Equals(@UserGuid)";
                    //LambdaExpression lambda = System.Linq.Dynamic.DynamicExpression.ParseLambda(typeof(T), typeof(bool), expression, Guid.NewGuid());
                    ////LambdaExpression lambda = System.Linq.Dynamic.DynamicExpression.ParseLambda(typeof(T), typeof(bool), expression, null);
                    //Expression<Func<T, bool>> predicate = (Expression<Func<T, bool>>)lambda;

                    //var test = GetFuncExpr(pr.FilterCriteria[i]);
                    //cursor = cursor.Where(predicate);

                    cursor = cursor.Where(pr.FilterCriteria[i].Predicate<T>());
                    //result.Items = result.Items.AsQueryable().Where(predicate).ToList();
                }
            }

            result.TotalRecords = cursor.Count();
            if (result.TotalRecords == 0)
            {
                result.PageRequest.FilterCriteria = null;
                result.PageRequest.SortCriteria = null;

                return result;
            }

            if (pr.SortCriteria != null && pr.SortCriteria.Count>0)
            {
                Func<IEnumerable<T>, IOrderedEnumerable<T>> sortFunc = null;

                for(int i=0;i<pr.SortCriteria.Count;i++)
                {
                    Func<T, object> orderByFunc = GetOrderFunction(pr.SortCriteria[i].PropertyName);
                    if (orderByFunc == null)
                        continue;

                    
                    if (pr.SortCriteria[i].Ascending)
                    {
                        sortFunc = (source => source.OrderBy(orderByFunc));
                    }
                    else
                    {
                        sortFunc = (source => source.OrderByDescending(orderByFunc));
                    }
                }

                result.Items = sortFunc(cursor).Skip(pr.PageIndex * pr.RowsPerPage).Take(pr.RowsPerPage).ToList();

                result.PageRequest.FilterCriteria = null;
                result.PageRequest.SortCriteria = null;

                return result;
            }

               
        //return sortFunc(cursor).Skip(pi * ps).Take(ps).ToList();
            result.Items= cursor.Skip(pr.PageIndex * pr.RowsPerPage).Take(pr.RowsPerPage).ToList();
            result.PageRequest.FilterCriteria = null;
            result.PageRequest.SortCriteria = null;
            return result;
        }
    }
}