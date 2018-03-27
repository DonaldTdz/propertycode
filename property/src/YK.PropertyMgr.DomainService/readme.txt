说明：依赖于Aspose.Cells.dll，将Aspose.Cells.lic文件添加到bin目录，在应用程序启动的方法中，添加如下代码： 
			Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Cells.lic");

核心方法：
1. 导出Excel
		/// <summary>
        /// 导出Excel通用方法
        /// </summary>
        /// <typeparam name="T">需要导出的数据类型</typeparam>
        /// <param name="exportDatas">导出的数据集</param>
        /// <param name="excelTemplateModels">模版数据集</param>
        /// <param name="isEnglish">是否是英文，默认为中文</param>
        /// <returns>输出可直接导出的Workbook</returns>
        public static Workbook Export<T>(IEnumerable<T> exportDatas, IEnumerable<ExcelTemplateModel> excelTemplateModels, bool isEnglish = false)；
2. 导出模版（供用户下载）
		/// <summary>
        /// 将模版导出Excel供用户下载
        /// </summary>
        /// <param name="templateModels">模版数据集</param>
        /// <param name="workbook">如果是在已有的workbook里面新增，则传递workbook，常用于将导入错误数据输出给用户的情况</param>
        /// <param name="isErrorTemplate">是否输出错误行号，一般同workbook联合使用</param>
        /// <param name="isEnglish">是否是英文，默认为中文</param>
        /// <returns>结果Workbook</returns>
        public static Workbook ExportTemplate(IEnumerable<ExcelTemplateModel> templateModels, Workbook workbook = null, bool isErrorTemplate = false, bool isEnglish = false)；
3. 导入方法
		/// <summary>
        /// Excel导入数据，相同数据不更新数据库
        /// </summary>
        /// <param name="filePath">导入的Excel路径</param>
        /// <param name="templateModels">模版数据集</param>
        /// <param name="customerValidateMethod">用户自定义的行验证方法，（遍历的数据行，读取的Table，导入成功的Table，验证结果返回）</param>
        /// <param name="importOtherColumns">是否需要增加额外的列</param>
        /// <returns>导入结果</returns>
        public static ImportResult ImportFromExcels(string filePath, IEnumerable<ExcelTemplateModel> templateModels, Func<DataRow, DataTable, DataTable, CustomerValidateResult> customerValidateMethod, Dictionary<string, object> importOtherColumns)
4. 导入方法
		/// <summary>
        /// Excel导入数据，相同数据更新数据库，
        /// </summary>
        /// <param name="filePath">导入的Excel路径</param>
        /// <param name="templateModels">模版数据集</param>
        /// <param name="customerValidateMethod">用户自定义的行验证方法，（遍历的数据行，读取的Table，导入成功的Table，验证结果返回）</param>
        /// <param name="importOtherColumns">是否需要增加额外的列</param>
        /// <returns>导入结果</returns>
        public static ImportResult ImportFromExcelsWithFilter(string filePath, IEnumerable<ExcelTemplateModel> templateModels, Func<DataRow, DataTable, DataTable, CustomerValidateResult> customerValidateMethod, Dictionary<string, object> importOtherColumns)
5. 导入数据到Sql Server
		/// <summary>
        /// 批量导入数据库
        /// </summary>
        /// <param name="insertTable">导入的Table</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">数据库事务</param>
        /// <returns></returns>
        public static string BulkInsertToTable(DataTable insertTable, string tableName, SqlConnection conn, SqlTransaction trans);
6. 导入数据到Mysql
		/// <summary>
        /// 批量导入数据到Mysql
        /// </summary>
        /// <param name="importTable">导入的数据源</param>
        /// <param name="excelTemplateModels">模版数据集合</param>
        /// <param name="tableName">表名称</param>
        /// <param name="connectMySQL">Mysql连接字符串</param>
        /// <param name="extentionColumns">扩展导入字段</param>
        /// <returns>导入结果</returns>
        public static string ImportMysql(DataTable importTable, IEnumerable<ExcelTemplateModel> excelTemplateModels, string tableName,MySqlConnection connectMySQL,List<string> extentionColumns = null)
7. 导入数据到Mysql
		/// <summary>
        /// 批量导入数据到Mysql
        /// </summary>
        /// <param name="importTable">导入的数据源</param>
        /// <param name="tableColumns">导入的表名集合</param>
        /// <param name="tableName">表名称</param>
        /// <param name="connectMySQL">Mysql连接字符串</param>
        /// <returns>导入结果</returns>
        public static string ImportMysql(DataTable importTable, List<string> tableColumns, string tableName, MySqlConnection connectMySQL)

/***********导入示例*****************

测试数据库脚本：
DROP TABLE IF EXISTS `testtable`;
CREATE TABLE `testtable` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Text` varchar(200) DEFAULT NULL,
  `Value` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4;

测试方法：
        public void ImportMysqlTest()
        {
            string strCnnMysql = "server=localhost;port=3306;database=YKFramework;uid=root;password=123456";

            DataTable orderDetail = new DataTable();
            DataColumn c = new DataColumn();
            orderDetail.Columns.Add(new DataColumn("Value"));
            orderDetail.Columns.Add(new DataColumn("Text"));

            for (int i=0;i< 10;i++)
            {
                DataRow dr = orderDetail.NewRow();
                dr["Text"] = "Test" + i;
                dr["Value"] = "Value" + i;
                orderDetail.Rows.Add(dr);
            }

            IEnumerable<ExcelTemplateModel> excelTemplateModels = new List<ExcelTemplateModel>()
            {
                new ExcelTemplateModel()
                {
                     Field = "Text",
                     CnName = "Text",
                     IsExport = true
                },
                new ExcelTemplateModel()
                {
                     Field = "Value",
                     CnName = "Value",
                     IsExport = true
                },
            };

            using (MySqlConnection cnnMysql = new MySqlConnection(strCnnMysql))
            {
                cnnMysql.Open();
                using (MySqlTransaction sqlTran = cnnMysql.BeginTransaction())
                {
                    string strResult = ExcelHelper.ImportMysql(orderDetail, excelTemplateModels, "TestTable", cnnMysql);
                    if (!string.IsNullOrEmpty(strResult))
                    {
                        sqlTran.Rollback();
                    }
                    else
                    {
                        sqlTran.Commit();
                    }
                }
            }
        }
