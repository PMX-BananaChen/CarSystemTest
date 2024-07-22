using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CarSystemTest.DBUtility
{
    public class StatementNode
    {
        private string title;
        private bool isProcedure;     //whether or not stored procedure        
        private string comment;
        private string sql;     //the concrete sql statement

        private IList<StatementParamNode> paramlst = new List<StatementParamNode>();
                
        public string Title
        {
            get 
            { 
                return title;
            }
            set
            { 
                title = value; 
            }
        }

        public bool IsProcedure
        {
            get 
            { 
                return isProcedure;
            }
            set
            { 
                isProcedure = value; 
            }
        }

        public string Comment
        {
            get
            {
                return comment;
            }
            set
            { 
                comment = value;
            }
        }

        public string Sql
        {
            get 
            { 
                return sql;
            }
            set
            { 
                sql = value; 
            }
        }

        public IList<StatementParamNode> Paramlst
        {
            get 
            {
                return paramlst; 
            }
            set 
            { 
                paramlst = value; 
            }
        }
    }
}

