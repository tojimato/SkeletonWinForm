using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Linq.Mapping;

namespace TSQL
{
    /// <summary>
    /// TODO : // RESULT CLASS EKLENECEK
    /// </summary>
    /// <typeparam name="Context"></typeparam>
    public class TSQL<Context>
        where Context : System.Data.Linq.DataContext, new()
    {
        Context obj;

        public TSQL()
        {
            this.obj = new Context();
        }
        /// <summary>
        /// Select
        /// </summary>
        /// <typeparam name="Class"></typeparam>
        /// <returns></returns>
        public IQueryable<Class> fetch<Class>(Expression<Func<Class, bool>> _where) where Class : class,new()
        {

            try
            {
                return this.obj.GetTable<Class>()
                                          .Where(_where);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Custom RunQuery
        /// </summary>
        /// <typeparam name="Class"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Result<IList<Class>> runQuery<Class>(string query, params object[] parameters) where Class : class
        {
            var result = new Result<IList<Class>>();
            try
            {
                result.TransactionResult = obj.ExecuteQuery<Class>(query, parameters).ToList();
                result.IsSucceeded = true;
            }
            catch (Exception ex)
            {

                result.IsSucceeded = false;
                result.TransactionException = ex;
                result.UserMessage.Add(ex.Message);
            }
            return result;
        }
 
        /// <summary>
        /// Insert Class
        /// </summary>
        /// <typeparam name="Class"></typeparam>
        /// <param name="cls"></param>
        /// <returns></returns>
        public Result<Class> insert<Class>(Class cls) where Class : class
        {
            var result = new Result<Class>();

            try
            {
                this.obj.GetTable<Class>().InsertOnSubmit(cls);
                this.obj.SubmitChanges();
                result.IsSucceeded = true;
            }
            catch (Exception ex)
            {
                this.obj = new Context();
                result.IsSucceeded = false;
                result.TransactionException = ex;
                result.UserMessage.Add(ex.Message);
            }

            return result;
        }
        /// <summary>
        ///    Update row with specified class object.
        /// </summary>
        /// <typeparam name="Class"></typeparam>
        /// <param name="cls"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Result<Class> update<Class>(Class cls, Expression<Func<Class, bool>> expression) where Class : class , new()
        {
            var result = new Result<Class>();
            try
            {
                var obj = this.obj;
                var itemList = obj.GetTable<Class>().Where(expression);

                foreach (var item in itemList)
                {
                    var currentItemProperties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    var primaryKeys = this.getPrimaryKey(cls);
                    foreach (var pk in primaryKeys)
                    {
                        foreach (PropertyInfo prop in currentItemProperties)
                        {
                            if (pk.Name != prop.Name)
                            {
                                if (null != prop && prop.CanWrite)
                                {
                                    var setValue = cls.GetType().GetProperty(prop.Name).GetValue(cls, null);
                                    if (setValue != null)
                                    {
                                        prop.SetValue(item, cls.GetType().GetProperty(prop.Name).GetValue(cls, null), null);
                                    }
                                }
                            }
                        }
                    }

                    var xx = obj.GetChangeSet();
                    obj.SubmitChanges();
                }
                result.IsSucceeded = true;
            }
            catch (Exception ex)
            {
                this.obj = new Context();
                result.IsSucceeded = false;
                result.TransactionException = ex;
                result.UserMessage.Add(ex.Message);
            }
            return result;
        }
        /// <summary>
        /// Delete Class
        /// </summary>
        /// <typeparam name="Class"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Result<Class> delete<Class>(Expression<Func<Class, bool>> expression) where Class : class
        {
            var result = new Result<Class>();

            try
            {
                var objects = this.obj.GetTable<Class>().Where(expression);
                var deletionObj = this.obj.GetTable<Class>();
                foreach (var item in objects)
                {
                    deletionObj.DeleteOnSubmit(item);
                }
                this.obj.SubmitChanges();
                result.IsSucceeded = true;
            }
            catch (Exception ex)
            {
                this.obj = new Context();
                result.IsSucceeded = false;
                result.TransactionException = ex;
                result.UserMessage.Add(ex.Message);
            }
            return result;
        }
        private IEnumerable<MetaDataMember> getPrimaryKey<Class>(Class cls) where Class : class
        {

            MetaType metaEntityType = this.obj.Mapping.GetMetaType(cls.GetType());

            var primaryKeyColumns = from pkColumn in metaEntityType.DataMembers
                                    where pkColumn.IsPrimaryKey
                                    select pkColumn;

            return primaryKeyColumns;
        }


    }
}
