using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.DataRequest;
using SchedulerEngine.Request;
using SchedulerEngine.Renderer;

namespace SchedulerEngine.Base
{
    /// <summary>
    /// Represent the base request for all scheduler requests
    /// </summary>
    public abstract class BaseRequest
    {
        protected IntervalsWrapper intervalsWrapper;
        protected SchedulerIntervals intervals;
        
        /// <summary>
        /// Returns a class containing the scheduler intervals
        /// </summary>
        /// <returns></returns>
        internal SchedulerRequest CreateSchedulerRequest()
        {
            SchedulerRequest request = new SchedulerRequest();

            //SchedulerRequestRenderer request = new SchedulerRequestRenderer();
            if (intervals != null)
            {
                request.SchedulerIntervals = intervals;
            }
            else
            {

                request.SchedulerIntervals.StartDate = intervalsWrapper.StartDateValue;

                request.SchedulerIntervals.MaxOccurencesNo = intervalsWrapper.MaxOccurencesNoValue;
                request.SchedulerIntervals.EndDate = intervalsWrapper.EndDateValue;

                request.SchedulerIntervals.IntervalPattern = intervalsWrapper.IntervalPatternValue;
            }
            return request;

        }

        /// <summary>
        /// call the Scheduler Request renderer in order to obtain the list of scheduler values
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal SchedulerResponse GetRecurringInfo(SchedulerRequest request)
        {
            SchedulerResponse values = SchedulerRequestRenderer.GetRecurringInfo(request);
            return values;
        }
        //protected SchedulerResponse CallService(SchedulerRequest request)
        //{

        //    SchedulerServiceClient client = new SchedulerServiceClient();

        //    SchedulerResponse resp = client.GetRecurringInfo(request);
        //    client.Close();


        //    return resp;
        //}
    }
}
