using System;
using System.Collections.Generic;
using System.Text;

namespace CarSystemTest.DBUtility
{
    public class StatementParamNode
    {
        private string name;        
        private string pos;        
        private string dbType;        
        private string direction;        
        private string size;

        public string Name
        {
            get
            {
                return name; 
            }
            set 
            {
                name = value;
            }
        }

        public string Pos
        {
            get 
            {
                return pos; 
            }
            set
            { 
                pos = value;
            }
        }

        public string DbType
        {
            get 
            { 
                return dbType;
            }
            set 
            { 
                dbType = value; 
            }
        }

        public string Direction
        {
            get
            { 
                return direction; 
            }
            set 
            { 
                direction = value; 
            }
        }

        public string Size
        {
            get
            { 
                return size; 
            }
            set 
            { 
                size = value;
            }
        }
    }
}
