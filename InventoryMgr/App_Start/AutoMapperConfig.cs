using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using InventoryMgr.Models;
using InventoryMgr.Models.ViewModels;

namespace InventoryMgr
{
    public static class AutoMapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<InventoryItem, InventoryItemEditModel>();
            Mapper.CreateMap<InventoryItemEditModel, InventoryItem>();
        }
    }
}