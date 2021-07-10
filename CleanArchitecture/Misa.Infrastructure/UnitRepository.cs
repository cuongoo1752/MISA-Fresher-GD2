﻿using Dapper;
using Microsoft.Extensions.Configuration;
using Misa.Core.Entities.Category;
using Misa.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Infrastructure
{
    public class UnitRepository : BaseEntityRepository<Unit>, IUnitRepository
    {
        #region DECLARE
        string _connectionString;
        IConfiguration _configuration;
        DynamicParameters Parameters;
        #endregion

        #region CONSTRUCTOR

        public UnitRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            Parameters = new DynamicParameters();
            InitConnection();
        }

        #endregion
    }
}
