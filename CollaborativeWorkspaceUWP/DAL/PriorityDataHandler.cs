using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class PriorityDataHandler
    {
        public PriorityDataHandler() { }

        public List<PriorityData> GetPriorityDataForUI()
        {
            List<PriorityData> result = new List<PriorityData>();
            try
            {
                foreach (Priority priority in Enum.GetValues(typeof(Priority)).Cast<Priority>())
                {
                    PriorityData priorityData = new PriorityData();
                    priorityData.Priority = priority.ToString();
                    priorityData.PriorityValue = (int)priority;
                    result.Add(priorityData);
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
            return result;
        }
    }
}
