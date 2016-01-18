﻿namespace Dixin.Linq.LinqToSql
{
    using System.Data.Linq;
    using System.Data.Linq.Mapping;

    using Dixin.Properties;

    [Database(Name = "[AdventureWorks]")]
    public partial class AdventureWorks : DataContext
    {
        public AdventureWorks()
            : base(Settings.Default.AdventureWorksConnectionString)
        {
            // if (!this.DatabaseExists())
            // {
            //    this.CreateDatabase();
            // }
        }
    }
}
