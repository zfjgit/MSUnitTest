namespace MSUnitTestProject.UserInfoUpdate.RowData
{
    public abstract class AbstractRowData
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public string Phone { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public string PyName { get; set; }

        public string EnName
        {
            get
            {
                if (Name.IndexOf("(") != -1)
                {
                    return Name.Substring(Name.IndexOf("(")).Trim();
                }
                return PyName;
            }
        }

        public string CnName
        {
            get
            {
                if (Name.IndexOf("(") != -1)
                {
                    return Name.Substring(0, Name.IndexOf("(")).Trim();
                }
                return Name;
            }
        }
    }
}