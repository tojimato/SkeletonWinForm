using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Linq.Expressions;

namespace TSQL
{
    public class TSQL<Context>
        where Context : System.Data.Linq.DataContext, new()
    {
        Context obj;

        public TSQL()
        {
            this.obj = new Context();
            if (this.obj.Connection.State == ConnectionState.Open)
                this.obj.Connection.Close();
        }

        //Generic get table classss
        public IQueryable<Class> fetch<Class>() where Class : class
        {
            try
            {
                return (from i in this.obj.GetTable<Class>() select i);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //Custom Run Query 
        public IList<Class> runQuery<Class>(string query, params object[] parameters) where Class : class
        {
            try
            {
                IList<Class> result;
                result = obj.ExecuteQuery<Class>(query, parameters).ToList();
                return result;
            }
            catch (Exception ex)
            {               
                throw new Exception(ex.Message);
            }

        }
        //Insert Class
        public bool insert<Class>(Class cls) where Class : class
        {
            var result = false;

            try
            {
                this.obj.GetTable<Class>().InsertOnSubmit(cls);
                this.obj.SubmitChanges();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                
            }

            return result;
        }
        //Update Class
        public bool update<Class>(Class cls, Expression<Func<Class, bool>> expression) where Class : class
        {

            var result = false;
            try
            {
                var rmv = this.delete(expression);
                if (rmv)
                {
                    this.insert<Class>(cls);
                }
                result = true;
            }
            catch (Exception ex)
            {
              
                result = false;
               
            }
            return result;
        }
        //Delete Class
        public bool delete<Class>(Expression<Func<Class, bool>> expression) where Class : class
        {
            var result = false;
            try
            {
                var objects = this.obj.GetTable<Class>().Where(expression);
                var deletionObj = this.obj.GetTable<Class>();
                foreach(var item in objects)
                {
                   deletionObj.DeleteOnSubmit(item);
                }
                this.obj.SubmitChanges();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


    }
}
