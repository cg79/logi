using MongoRepository.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Pagination
{
    public class FilterCriteria
    {
        public string FieldName { get; set; }
        public Operator Operator { get; set; }
        public object Value { get; set; }
        public Type ValueType { get; set; }
        public int Index { get; set; }
       
        public string Expression
        {
            get
            {
                string val = null;

                var ts = new TypeSwitch()
                .CaseType( typeof(int),() => val = Value.ToString())
                .CaseType(typeof(bool) , ()=> val = Value.ToString())
                .CaseType(typeof(string), () => val = string.Concat("\"", Value.ToString(), "\""))
                //.CaseType(typeof(Guid), () => val = string.Concat("Guid.Parse(", string.Concat("\"", Value.ToString(), "\""), ")"))
                .CaseType(typeof(Guid), () => val =  Value.ToString())
                ;
                try
                {
                    ts.SwitchType(ValueType);
                }
                catch (Exception ex)
                {
 
                }
                if(val == null)
                {
                    val = string.Concat("\"",Value.ToString(),"\"");
                }
                return string.Concat( FieldName , " == " ,val);
            }
        }

        public Expression<Func<T, bool>> Predicate<T>()
        {
            string val = "";
            Expression<Func<T, bool>> predicate = null;
            var ts = new TypeSwitch()
                .CaseType(typeof(int), () => val = Value.ToString())
                .CaseType(typeof(bool), () => val = Value.ToString())
                .CaseType(typeof(string), () => 
                    {
                        val = string.Concat(FieldName, " == ","\"",Value.ToString(),"\"");
                        LambdaExpression lambda = System.Linq.Dynamic.DynamicExpression.ParseLambda(typeof(T), typeof(bool), val, null);
                        predicate = (Expression<Func<T, bool>>)lambda;

                    })
                //.CaseType(typeof(Guid), () => val = string.Concat("Guid.Parse(", string.Concat("\"", Value.ToString(), "\""), ")"))
                .CaseType(typeof(Guid), () => 
                    {
                        val = string.Concat(FieldName, " == @", Index.ToString());
                        LambdaExpression lambda = System.Linq.Dynamic.DynamicExpression.ParseLambda(typeof(T), typeof(bool), val, Guid.Parse(Value.ToString()));
                        predicate = (Expression<Func<T, bool>>)lambda;
                    })
                ;
            try
            {
                ts.SwitchType(ValueType);
            }
            catch (Exception ex)
            {

            }
            return predicate;
        }
    }
}
