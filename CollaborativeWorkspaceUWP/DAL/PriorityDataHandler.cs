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

        public List<Priority> GetPriorityDataForUI()
        {
            List<Priority> result = new List<Priority>();
            try
            {
                foreach (PriorityEnum priority in Enum.GetValues(typeof(PriorityEnum)).Cast<PriorityEnum>())
                {
                    Priority priorityData = new Priority();
                    priorityData.Name = priority.ToString();
                    priorityData.Value = (int)priority;
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
