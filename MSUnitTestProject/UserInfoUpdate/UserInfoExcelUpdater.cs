using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MSUnitTestProject.NPinyin;
using MSUnitTestProject.UserInfoUpdate.Handler;
using MSUnitTestProject.UserInfoUpdate.Handler.UserValidate;
using MSUnitTestProject.UserInfoUpdate.RowData;
using MSUnitTestProject.UserInfoUpdate.Service;
using MSUnitTestProject.UserInfoUpdate.UserData;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Sheet1 = MSUnitTestProject.UserInfoUpdate.Handler.RowValidate.Sheet1;
using Sheet2 = MSUnitTestProject.UserInfoUpdate.Handler.RowValidate.Sheet2;

namespace MSUnitTestProject.UserInfoUpdate
{
    public class UserInfoExcelUpdater
    {
        private static readonly IUserService s_userService = new Mock_UserService();
        private static readonly Dictionary<short, string> s_colors = new Dictionary<short, string>
        {
            { HSSFColor.Rose.Index, "粉红色：空行" },
            { HSSFColor.Lavender.Index , "浅紫色：部门行" },
            { HSSFColor.Grey25Percent.Index, "浅灰色：标题行" },
            { HSSFColor.White.Index , "白色：所属公司ID不等于1"  },
            { HSSFColor.LightOrange.Index , "浅橙色：姓名为空" } ,
            { HSSFColor.PaleBlue.Index , "浅蓝色：更新同名用户" },
            { HSSFColor.Aqua.Index, "水蓝色：更新单个用户成功" },
            { HSSFColor.Grey50Percent.Index, "深灰色：用户不存在"  },
            { HSSFColor.Red.Index , "红色：更新用户失败"  },
        };

        private static readonly Dictionary<short, int> s_counts = new Dictionary<short, int>
        {
            { HSSFColor.Rose.Index, 0 },
            { HSSFColor.Lavender.Index , 0 },
            { HSSFColor.LightOrange.Index , 0 } ,
            { HSSFColor.Grey25Percent.Index, 0 },
            { HSSFColor.Grey50Percent.Index, 0  },
            { HSSFColor.PaleBlue.Index , 0 },
            { HSSFColor.Aqua.Index, 0 },
            { HSSFColor.Red.Index , 0  },
            { HSSFColor.White.Index , 0  },
        };

        private static readonly Dictionary<string, IList<User>> s_duplicateUsers = new Dictionary<string, IList<User>>();

        public static void Update()
        {
            try
            {
                string absPath = "D:/Documents/UserInfo";

                XSSFWorkbook workbook = null;
                using (FileStream fs = new FileStream(absPath + ".xlsx", FileMode.Open, FileAccess.ReadWrite))
                {
                    workbook = new XSSFWorkbook(fs);
                }

                UpdateSheet1(workbook.GetSheetAt(0));

                UpdateSheet2(workbook.GetSheetAt(1));

                SaveWorkbook(workbook, absPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw ex;
            }
        }

        private static void UpdateSheet1(ISheet sheet)
        {
            Console.WriteLine("Updating sheet 1...");

            InitCount();

            InitDuplicateUsers();

            var rowValidHandler = InitSheet1RowValidHandlers();

            var userValidHandler = InitUserValidHandlers();

            var userUpdateResultHandler = InitUpdateResultHandlers();

            for (int rowIdx = 0; rowIdx <= sheet.LastRowNum; rowIdx++)
            {
                IRow row = sheet.GetRow(rowIdx);
                var rowData = new Sheet1RowData();

                dynamic context = new HandleContext<Sheet1RowData>(row, rowData);
                if (!rowValidHandler.Handle(context))
                {
                    continue; // or start another handler chain
                }

                Console.WriteLine($"Excel row: {rowData.Name} {rowData.Job} {rowData.Telephone} {rowData.Email}");

                rowData.PyName = Pinyin.GetPinyin(rowData.CnName);

                IList<string> names = GetNameParams(rowData);
                IList<User> users = s_userService.FindUsers(names);

                context = new HandleContext<IList<User>>(row, users, rowData.Name);
                if (!userValidHandler.Handle(context))
                {
                    continue; // or start another handler chain
                }

                context.ResultCount = UpdateUser(users, rowData);

                userUpdateResultHandler.Handle(context);
            }

            Console.WriteLine("Sheet 1 updated.");

            CreateColorDescriptionRow(sheet);
        }

        private static void UpdateSheet2(ISheet sheet)
        {
            Console.WriteLine("Updating sheet 2...");

            InitCount();

            InitDuplicateUsers();

            var rowValidHandler = InitSheet2RowValidHandlers();

            var userValidHandler = InitUserValidHandlers();

            var userUpdateResultHandler = InitUpdateResultHandlers();

            for (int rowIdx = 0; rowIdx <= sheet.LastRowNum; rowIdx++)
            {
                IRow row = sheet.GetRow(rowIdx);
                var rowData = new Sheet2RowData();

                dynamic context = new HandleContext<Sheet2RowData>(row, rowData);
                if (!rowValidHandler.Handle(context))
                {
                    continue;
                }

                Console.WriteLine($"Excel row: {rowData.Name} {rowData.PyName} {rowData.Job} {rowData.Phone} {rowData.Telephone} {rowData.Email}");

                IList<string> names = GetNameParams(rowData);
                IList<User> users = s_userService.FindUsers(names);

                context = new HandleContext<IList<User>>(row, users, rowData.Name);
                if (!userValidHandler.Handle(context))
                {
                    continue;
                }

                context.ResultCount = UpdateUser(users, rowData);

                userUpdateResultHandler.Handle(context);
            }

            Console.WriteLine("Sheet 2 updated.");

            CreateColorDescriptionRow(sheet);
        }

        private static IList<string> GetNameParams(AbstractRowData rowData)
        {
            string[] pyNameAry = rowData.PyName.Split(' ');
            string firstName = pyNameAry[0];
            string lastName = pyNameAry[1] + (pyNameAry.Length > 2 ? pyNameAry[2] : "");

            return new List<string>{
                        rowData.Name,
                        rowData.CnName,
                        rowData.EnName,
                        rowData.PyName,

                        firstName + "" + lastName,
                        firstName + "," + lastName,
                        firstName + "." + lastName,
                        firstName + " " + lastName,
                        firstName + ", " + lastName,
                        firstName + ", " + lastName,

                        lastName + "" + firstName,
                        lastName + "," + firstName,
                        lastName + "." + firstName,
                        lastName + " " + firstName,
                        lastName + ", " + firstName,
                        lastName + ". " + firstName,

                    };
        }

        private static int UpdateUser(IList<User> users, AbstractRowData rowData)
        {
            int r = 0;
            foreach (var user in users)
            {
                if (user.CompanyID != 1)
                {
                    continue;
                }

                Console.WriteLine($"User info: {user.CnName} {user.FullName} {user.Job} {user.Phone} {user.Email}");

                user.CnName = rowData.CnName;
                user.FullName = rowData.PyName;
                user.Email = rowData.Email;
                user.Phone = rowData.Telephone;
                user.Job = rowData.Job;

                r += s_userService.Save(user);
            }
            return r;
        }

        private static void SaveWorkbook(XSSFWorkbook workbook, string absPath)
        {
            FileInfo file = new FileInfo(absPath + "-1.xlsx");
            if (file.Exists)
            {
                file.Delete();
            }

            using (FileStream fs = file.Create())
            {
                workbook.Write(fs);
            }
        }

        private static void InitDuplicateUsers()
        {
            s_duplicateUsers.Clear();
        }

        private static void InitCount()
        {
            foreach (var key in s_counts.Keys.ToArray())
            {
                s_counts[key] = 0;
            }
        }

        private static void CreateColorDescriptionRow(ISheet sheet)
        {
            IRow row0 = sheet.CreateRow(0);
            row0.Height = 1000;

            sheet.RemoveMergedRegion(0);

            for (int i = 0; i < s_colors.Count; i++)
            {
                KeyValuePair<short, string> keyValuePair = s_colors.ElementAt(i);

                ICell cell = row0.CreateCell(i);

                StringBuilder sbd = new StringBuilder();
                if (keyValuePair.Key == HSSFColor.PaleBlue.Index)
                {
                    sbd.Append(", 重复: \r");
                    foreach (var item in s_duplicateUsers)
                    {
                        sbd.Append(item.Key).Append(": ");
                        for (int j = 0; j < item.Value.Count; j++)
                        {
                            sbd.Append("'");
                            sbd.Append(item.Value[j].Name);
                            sbd.Append("'");
                            if (j < item.Value.Count - 1)
                            {
                                sbd.Append(", ");
                            }
                        }
                        sbd.Append("\r");
                    }
                    Console.WriteLine(sbd.ToString());
                }

                cell.SetCellValue(keyValuePair.Value + ", 共: " + s_counts[keyValuePair.Key] + sbd);

                if (i > 4 && sheet.GetRow(3).LastCellNum < 6)
                {
                    cell.Sheet.SetColumnWidth(i, 8000);
                }

                if (i > 6 && sheet.GetRow(3).LastCellNum > 6)
                {
                    cell.Sheet.SetColumnWidth(i, 8000);
                }

                SetCellBgColor(cell, keyValuePair.Key);
            }
        }
        private static void SetCellBgColor(ICell cell, short colorIndex)
        {
            cell.CellStyle = cell.Sheet.Workbook.CreateCellStyle();

            cell.CellStyle.Alignment = HorizontalAlignment.Center;
            cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;

            cell.CellStyle.FillForegroundColor = colorIndex;
            cell.CellStyle.FillPattern = FillPattern.SolidForeground;
        }

        private static UserUpdateSuccessResultHandler InitUpdateResultHandlers()
        {
            var userUpdateFailedResultHandler = new UserUpdateFailedResultHandler(HSSFColor.Red.Index, s_counts, null);
            var userUpdateSuccessResultHandler = new UserUpdateSuccessResultHandler(HSSFColor.Aqua.Index, s_counts, userUpdateFailedResultHandler);
            return userUpdateSuccessResultHandler;
        }

        private static UserNotFoundHandler InitUserValidHandlers()
        {
            var userDuplicationHandler = new UserDuplicationHandler(HSSFColor.PaleBlue.Index, s_counts, s_duplicateUsers, null);
            var userCompanyIDHandler = new UserCompanyIDHandler(HSSFColor.White.Index, s_counts, userDuplicationHandler);
            var userNotFoundHandler = new UserNotFoundHandler(HSSFColor.Grey50Percent.Index, s_counts, userCompanyIDHandler);
            return userNotFoundHandler;
        }

        private static Sheet1.RowValidHandler InitSheet1RowValidHandlers()
        {
            var nameValidHandler = new Sheet1.NameValidHandler(0, HSSFColor.LightOrange.Index, s_counts, null);
            var deptNameRowHandler = new Sheet1.DeptNameRowHandler(1, HSSFColor.Lavender.Index, s_counts, nameValidHandler);
            var titleRowHandler = new Sheet1.TitleRowHandler(0, HSSFColor.Grey25Percent.Index, s_counts, deptNameRowHandler);
            var rowValidHandler = new Sheet1.RowValidHandler(5, -1, HSSFColor.Rose.Index, s_counts, titleRowHandler);
            return rowValidHandler;
        }

        private static Sheet2.RowValidHandler InitSheet2RowValidHandlers()
        {
            var nameValidHandler = new Sheet2.NameValidHandler(0, HSSFColor.LightOrange.Index, s_counts, null);
            var deptNameRowHandler = new Sheet2.DeptNameRowHandler(1, HSSFColor.Lavender.Index, s_counts, nameValidHandler);
            var titleRowHandler = new Sheet2.TitleRowHandler(0, HSSFColor.Grey25Percent.Index, s_counts, deptNameRowHandler);
            var rowValidHandler = new Sheet2.RowValidHandler(5, -1, HSSFColor.Rose.Index, s_counts, titleRowHandler);
            return rowValidHandler;
        }
    }
}