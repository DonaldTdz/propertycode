using System;
using System.Data;
using ZY_DBHelper;
using ZY_WebLibrary;
namespace PropertySysAPI.Entity
{
    [Table("ShareKeys", "Id", "Id")]
    public class ShareKeys
    {
        #region 属性 
        /// <summary> 
        ///  
        /// <summary> 
        [Field("Id", "int", "10", Key.id_primary)]
        public int? Id { get; set; }

        /// <summary> 
        ///  
        /// <summary> 
        [Field("UserId", "varchar", "20", Key.field)]
        public string UserId { get; set; }

        /// <summary> 
        ///  
        /// <summary> 
        [Field("Keys", "nvarchar", "-1", Key.field)]
        public string Keys { get; set; }

        /// <summary> 
        ///  
        /// <summary> 
        [Field("SetNums", "int", "10", Key.field)]
        public int? SetNums { get; set; }

        /// <summary> 
        ///  
        /// <summary> 
        [Field("UseNums", "int", "10", Key.field)]
        public int? UseNums { get; set; }

        /// <summary> 
        ///  
        /// <summary> 
        [Field("KeyDate", "date", "10", Key.field)]
        public DateTime KeyDate { get; set; }

        /// <summary> 
        ///  
        /// <summary> 
        [Field("UpdateTime", "datetime", "23", Key.field)]
        public DateTime UpdateTime { get; set; }

        /// <summary> 
        ///  
        /// <summary> 
        [Field("CreateTime", "datetime", "23", Key.field)]
        public DateTime CreateTime { get; set; }

        #endregion

        #region 方法 
        public ShareKeys()
        {
            this.Id = null;
            this.UserId = null;
            this.Keys = null;
            this.SetNums = null;
            this.UseNums = null;
            this.KeyDate = DateTime.MinValue;
            this.UpdateTime = DateTime.MinValue;
            this.CreateTime = DateTime.MinValue;
        }

        public ShareKeys(IDataReader read)
        {
            try
            {
                this.Id = WebTool.QeryNumber(read["Id"]);
                this.UserId = WebTool.QeryString(read["UserId"]);
                this.Keys = WebTool.QeryString(read["Keys"]);
                this.SetNums = WebTool.QeryNumber(read["SetNums"]);
                this.UseNums = WebTool.QeryNumber(read["UseNums"]);
                this.KeyDate = WebTool.QeryDateTime(read["KeyDate"]);
                this.UpdateTime = WebTool.QeryDateTime(read["UpdateTime"]);
                this.CreateTime = WebTool.QeryDateTime(read["CreateTime"]);
            }
            catch { }
        }
        #endregion
    }

}
