using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class StatusDataHandler
    {
        public StatusDataHandler() { }

        public List<Status> GetStatusData()
        {
            List<Status> result = new List<Status>();
            try
            {
                foreach (StatusEnum status in Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>())
                {
                    Status statusData = new Status();
                    statusData.Name = status.ToString();
                    statusData.Id = (int)status;
                    result.Add(statusData);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return result;
        }
    }
}
