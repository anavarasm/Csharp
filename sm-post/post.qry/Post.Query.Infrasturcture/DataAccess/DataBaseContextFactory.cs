using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrasturcture.DataAccess
{
    public class DataBaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public DataBaseContextFactory (Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public DataBaseContext createDbContext()
        {
            DbContextOptionsBuilder<DataBaseContext> optionsBuilder = new();
            _configureDbContext(optionsBuilder);
            return new DataBaseContext(optionsBuilder.Options);
        }
    }
}
