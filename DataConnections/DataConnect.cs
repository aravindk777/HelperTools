using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DataConnections
{
    public class DataConnect
    {

        string envType;

        public DataConnect()
        {
            //envType = 1; //set as DEV by default
        }

        public DataConnect(string environmenttype)
        {
            envType = environmenttype;
        }

        public AgentProfileDataSet GetAgentProfileData(string UserId)
        {
            string ConnectionString = string.Empty;

            switch(envType)
            {
                case "DEV":
                default:
                    ConnectionString = ConfigurationManager.ConnectionStrings["NLG_WI_NationalLifeEntities_DEV"].ConnectionString;
                    break;

                case "UAT":
                    ConnectionString = ConfigurationManager.ConnectionStrings["NLG_WI_NationalLifeEntities_UAT"].ConnectionString;
                    break;

                case "PROD":
                    ConnectionString = ConfigurationManager.ConnectionStrings["NLG_WI_NationalLifeEntities_PROD"].ConnectionString;
                    break;
            }

            NLG_WI_NationalLifeEntities ent = new NLG_WI_NationalLifeEntities(ConnectionString);

            return ent.usp_GetAgentProfileData(UserId).FirstOrDefault();
        }
    }
}
