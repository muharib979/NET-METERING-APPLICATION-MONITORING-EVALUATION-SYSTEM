using Core.Application.Interfaces.Building.RepositoryInterfaces;
using Core.Domain.Building;

using Core.Application.Common.Extensions;
using Shared.DTOs.Enums;
using AutoMapper;
using Oracle.ManagedDataAccess.Client;
using AdminSystem.Infrastructure.Persistence.Context;
using Shared.DTOs.Building;
using Core.Application.Common.Interfaces;
using System.Collections.Generic;

namespace CFEMS.Infrastructure.Persistence.Repositories.BuildingRepositories;

public class BuildingRepository : IBuildingRepository
{
    //private readonly IDbConnection _db;
    private readonly IMapper _mapper;
    //public BuildingRepository(DapperContext context) => _db = context.GetDbConnection();
    public BuildingRepository(DapperContext context, IMapper mapper) 
    {
        _mapper= mapper;
    }

    //public async Task<int> AddAsync(Building entity)
    //{
    //    using var con = new OracleConnection(Connection.ConnectionString());
    //    var ISS_INSTALL_DATE = DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss");
    //    var sql = @"insert into building (site_nbr, address_code, building_title, address, postal_code, building_type, asset_no, lta_id, iss_install_date, is_active, is_on_test) values (:SITE_NBR, :ADDRESS_CODE, :BUILDING_TITLE, :ADDRESS, :POSTAL_CODE, :BUILDING_TYPE, :ASSET_NO, :LTA_ID," + $" TO_DATE( '"+$"{ISS_INSTALL_DATE}" + "',  'YYYY-MM-DD HH24:MI:SS')," + @" :IS_ACTIVE, :IS_ON_TEST)";
    //    //var sql = @"Insert into INFONET.BUILDING (SITE_NBR,ADDRESS_CODE,BUILDING_TITLE,ADDRESS,POSTAL_CODE,BUILDING_TYPE,ASSET_NO,LTA_ID,ISS_INSTALL_DATE,IS_ACTIVE,IS_ON_TEST) values ('kjakl','1111','kjalkjf','jbkajk','1212','ajikj','24','1',to_date('28-NOV-22','DD-MON-RR'),0,1)";
    //    return await con.ExecuteAsync(sql, entity);
    //}

    public async Task<int> AddAsync(BuildingDto entity)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var ISS_INSTALL_DATE = DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss");
        var sql = @"insert into building (site_nbr, address_code, building_title, address, postal_code, building_type, asset_no, lta_id, iss_install_date, is_active, is_on_test) values (:SITE_NBR, :ADDRESS_CODE, :BUILDING_TITLE, :ADDRESS, :POSTAL_CODE, :BUILDING_TYPE, :ASSET_NO, :LTA_ID," + $" TO_DATE( '" + $"{ISS_INSTALL_DATE}" + "',  'YYYY-MM-DD HH24:MI:SS')," + @" :IS_ACTIVE, :IS_ON_TEST)";
        //var sql = @"Insert into INFONET.BUILDING (SITE_NBR,ADDRESS_CODE,BUILDING_TITLE,ADDRESS,POSTAL_CODE,BUILDING_TYPE,ASSET_NO,LTA_ID,ISS_INSTALL_DATE,IS_ACTIVE,IS_ON_TEST) values ('kjakl','1111','kjalkjf','jbkajk','1212','ajikj','24','1',to_date('28-NOV-22','DD-MON-RR'),0,1)";
            
        return await con.ExecuteAsync(sql, entity);
    }

    //public Task<int> AddListAsync(List<Building> entity)
    //{
    //    throw new NotImplementedException();
    //}

    public Task<int> AddListAsync(List<BuildingDto> entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

   

    public async Task<List<BuildingDto>> GetAllAsync(PaginationParams pParams)
    {
        List<BuildingDto> buildingDtos= new List<BuildingDto>();
        using var con = new OracleConnection(Connection.ConnectionString());
        string sql = @"Select * from (SELECT COUNT(*) OVER() as TotalRowCount, 
                                ROW_NUMBER() OVER (ORDER BY b.Id) AS RowNumber,
                                b.id, 
                                b.site_nbr, 
                                b.address_code,
                                b.building_title,
                                -- b.address, 
                                -- b.postal_code, 
                                b.building_type, 
                                b.iss_install_date
                                FROM building b ) temp";

        //if (pParams.SearchBy != null)
        //{
        //    sql += @" WHERE (b.col_building_title  like   '%' + @searchBy + '%' OR b.col_site_nbr like   '%' + @searchBy + '%' OR b.col_postal_code like   '%' + @searchBy + '%') AND b.col_is_active = 1";
        //}
        //else
        //{
        //    sql += @" WHERE b.col_is_active = 1";
        //}

        //sql += @$" where RowNumber BETWEEN :startRow AND :endRow";

        var buildings = await con.QueryAsync<Building>(sql, new { startRow = (pParams.PageNumber * pParams.pageSize) + 1, endRow = (pParams.pageSize * pParams.PageNumber), searchBy = pParams.SearchBy });
        buildingDtos = _mapper.Map(buildings.ToList(), buildingDtos);
        return buildingDtos;
    }

    public async Task<List<DropdownResult>> GetAllBuildingDDAsync()
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = $"SELECT id as Key, CONCAT(building_title,' (',site_nbr,')') as Value from building where is_active = 1"; // 1 is true
        var dDres = await con.QueryAsync<DropdownResult>(sql);
        return dDres.ToList();
    }

    public async Task<BuildingDto> GetByIdAsync(int id)
    {
        BuildingDto buildings = new BuildingDto();
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = @"SELECT b.id, 
                        b.site_nbr, 
                        b.address_code, 
                        b.building_title, 
                        b.address, 
                        b.postal_code, 
                        b.building_type, 
                        b.asset_no, 
                        b.lta_id, 
                        b.iss_install_date, 
                        b.is_active, 
                        b.is_on_test
                        FROM building b
                        WHERE b.id = :id AND is_active =:isActive";

        var building = await con.QueryAsync<Building>(sql, new { id, isActive = (int)BooleanEnum.TRUE });
        buildings = _mapper.Map(building.FirstOrDefault(), buildings);
        return buildings;
    }
    public async Task<int> GetTotalCountAsync(string searchBy)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        string countSql;
        if (searchBy == null)
        {
            countSql = @"SELECT COUNT(id)
                            FROM BUILDING WHERE is_active = 1";
        }
        else
        {
            countSql = @"SELECT COUNT (DISTINCT id)
                            FROM BUILDINGS as b
                            WHERE (b.building_title  like   '%' + @searchBy + '%' OR b.site_nbr like   '%' + @searchBy + '%' OR b.postal_code like   '%' + @searchBy + '%') AND b.is_active = 1";
        }

        var totalCount = await con.QueryAsync<int>(countSql, new { searchBy = searchBy });
        return totalCount.FirstOrDefault();
    }

    public async Task<bool> IsUniqueSiteNbrAsync(string sitenbr)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = @"SELECT
                    CASE WHEN EXISTS 
                    (
                          SELECT site_nbr FROM BUILDING WHERE site_nbr=:sitenbr limit 1
                    )
                    THEN TRUE ELSE FALSE END";

        var isExists = await con.QueryFirstOrDefaultAsync<bool>(sql, new { sitenbr = sitenbr });

        return isExists;
    }

    //public async Task<int> UpdateAsync(Building entity)
    //{
    //    var sql = @"update building set  site_nbr=:SITE_NBR, address_code=:ADDRESS_CODE, building_title=:BUILDING_TITLE, address=:ADDRESS, postal_code=:POSTAL_CODE, building_type=:BUILDING_TYPE, asset_no=:ASSSET_NO, lta_id=:LTA_ID, iss_install_date=:ISS_INSTALL_DATE, is_active=:IS_ACTIVE, is_on_test=:IS_ON_TEST where building_id=:BUILDING_ID";
    //    return await _db.ExecuteAsync(sql, entity);
    //}

    public async Task<int> UpdateAsync(BuildingDto entity)
    {
        using var con = new OracleConnection(Connection.ConnectionString());  
        var sql = @"update building set  site_nbr=:SITE_NBR, address_code=:ADDRESS_CODE, building_title=:BUILDING_TITLE, address=:ADDRESS, postal_code=:POSTAL_CODE, building_type=:BUILDING_TYPE, asset_no=:ASSSET_NO, lta_id=:LTA_ID, iss_install_date=:ISS_INSTALL_DATE, is_active=:IS_ACTIVE, is_on_test=:IS_ON_TEST where building_id=:BUILDING_ID";
        return await con.ExecuteAsync(sql, entity);
    }

}
