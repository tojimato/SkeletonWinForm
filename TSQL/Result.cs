using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Runtime.Serialization;

namespace TSQL
{
    /// <summary>
    /// Database üzerinde yapılan sorgulamaların dönüş standartlarını belirleyen sınıf
    /// </summary>
    /// 
    //[DataContract]
    public class Result
    {
        /// <summary>
        /// Result Durumu. Yapılan işlem sonucu Ture yada False olarak buraya yazılır.
        /// </summary>
        /// 
        //[DataMember]
        public bool IsSucceeded { get; set; }
        /// <summary>
        /// İşlem sonucunda oluşmuş olan Exception bu bölüme yazılır.
        /// </summary>
        private Exception _transactionException;
        //[DataMember]
        public Exception TransactionException
        {
            get
            {
                return _transactionException;
            }
            set
            {
                _transactionException = value;
                //_userMessage.Add(Exceptions.Exceptions.ExceptionSave(value));
            }
        }
        /// <summary>
        /// İşlem sonucunda kullanıcıya verilmek istenen mesaj bu bölüme yazılır. Hata yada olumlu iş mesajı olabilir.
        /// </summary>
        /// 
        private IList<string> _userMessage = new List<string>();
        //[DataMember]
        public IList<string> UserMessage
        {
            get
            {
                return _userMessage;
            }
            set
            {
                _userMessage = value;
            }
        }

        //[DataMember]
        public string UserMessageHtml
        {
            get
            {
                StringBuilder retval = new StringBuilder();
                foreach (string m in _userMessage)
                {
                    //retval.AppendLine("<div class=\"alert alert-info\">" + m + "</div>");
                    retval.AppendLine("* " + m);
                }

                if (this.TransactionException != null)
                {
                    retval.AppendLine(this.TransactionException.Message);
                }
                return retval.ToString();
            }
        }
    }
    //[DataContract]
    public class Result<T> : Result
    {
        /// <summary>
        /// Dönüş değerine göre generic tip oluşturulur.
        /// </summary>
        /// 
        //[DataMember]
        public T TransactionResult { get; set; }
    }
}
