﻿
using CoreLib.Inventory.Interfaces;
using System;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;

namespace Modules.Inventory
{
    public class ItemSpecServices : IItemSpecServices
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        //private readonly int TYPE_RECEIVED = 1;
        //private readonly int TYPE_RELEASED = 2;

        //private readonly int STATUS_APPROVED = 2;
        //private readonly int STATUS_CLOSED = 3;

        public ItemSpecServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }

        public virtual async Task<int> AddItemSpecification(InvItemSpec_Steel invItemSpec)
        {
            try
            {
                _context.InvItemSpec_Steel.Add(invItemSpec);
                return await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to AddItemSpecification" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to AddItemSpecification." + ex.Message);
            }
        }

        public virtual async Task<bool> CheckItemHasAnyInvSpec(int invItemId)
        {
            try
            {
               return await _context.InvItemSpec_Steel.AnyAsync(i => i.InvItemId == invItemId);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Error on checking CheckItemSpecHasAny" + ex.Message);
                throw new Exception("ItemSpecServices:  Error on checking CheckItemSpecHasAny." + ex.Message);
            }
        }

        public virtual async Task<int> DeleteItemSpecification(InvItemSpec_Steel invItemSpec)
        {
            try
            {
                _context.InvItemSpec_Steel.Remove(invItemSpec);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to DeleteItemSpecification" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to DeleteItemSpecification." + ex.Message);
            }
        }

        public virtual async Task<int> EditItemSpecification(InvItemSpec_Steel invItemSpec)
        {
            try
            {

                _context.Attach(invItemSpec).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to DeleteItemSpecification" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to DeleteItemSpecification." + ex.Message);
            }
        }


        public virtual void EditItemSpecificationOnly(InvItemSpec_Steel invItemSpec)
        {
            try
            {

                _context.Attach(invItemSpec).State = EntityState.Modified;

            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to DeleteItemSpecification" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to DeleteItemSpecification." + ex.Message);
            }
        }


        public virtual InvItemSpec_Steel GetItemSpecification(int id)
        {
            try
            {
                var invitemspec = _context.InvItemSpec_Steel.Find(id);

                _logger.LogInformation("ItemSpecServices: GetItemSpecification Done, itemspec ID: " + id);

                return invitemspec;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to DeleteItemSpecification" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to DeleteItemSpecification." + ex.Message);
            }
        }

        public virtual List<InvItemSpec_Steel> GetItemSpecification_ByInvItemId(int invItemId)
        {
            try
            {
                var invitemspec = _context.InvItemSpec_Steel.Where(i => i.InvItemId == invItemId).ToList();

                _logger.LogInformation("ItemSpecServices: GetItemSpecification Done, itemspec ID: " + invItemId);

                return invitemspec;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to DeleteItemSpecification" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to DeleteItemSpecification." + ex.Message);
            }
        }

        //Get Select List of Inventory Items, used for Create or Edit Dropdowns List
        public virtual IEnumerable<InvItemSysDefinedSpecs> GetDefinedSpecs()
        {
            return _context.InvItemSysDefinedSpecs.ToList();
        }


        public virtual bool IsCategoryHaveSpecDefs(int? categoryId)
        {
            if (categoryId == null)
            {
                return false;
            }

            var itemCategory = _context.InvCategorySpecDefs
                                   .Where(i => i.InvCategoryId == categoryId)
                                   .ToList();

            if (itemCategory.Count() > 0)
            {
                return true;
            }

            return false;
        }

        public virtual async Task<int> AddItemCustomSpecification(InvItemCustomSpec invItemCustomSpec)
        {
            try
            {
                _context.InvItemCustomSpecs.Add(invItemCustomSpec);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to AddItemCustomSpecification" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to AddItemCustomSpecification.");
            }
        }

        public virtual async Task<int> EditItemCustomSpecification(InvItemCustomSpec invItemCustomSpec)
        {
            try
            {
                _context.Attach(invItemCustomSpec).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to EditItemCustomSpecification" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to EditItemCustomSpecification.");
            }
        }

        public virtual async Task<int> DeleteItemCustomSpecification(InvItemCustomSpec invItemCustomSpec)
        {
            try
            {
                _context.InvItemCustomSpecs.Remove(invItemCustomSpec);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to DeleteItemCustomSpecification" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to DeleteItemCustomSpecification.");
            }
        }

        public virtual InvItemCustomSpec GetItemCustomSpecification(int id)
        {
            try
            {
                var invItemCustomSpec = _context.InvItemCustomSpecs.Find(id);

                return invItemCustomSpec;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to GetItemCustomSpecification" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to GetItemCustomSpecification." + ex.Message);
            }
        }

        public virtual List<InvItemCustomSpec> GetItemCustomSpecification_ByInvItemId(int invItemId)
        {
            try
            {
                var invItemCustomSpec = _context.InvItemCustomSpecs.Where(i => i.InvItemId == invItemId).ToList();

                return invItemCustomSpec;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to GetItemCustomSpecification_ByInvItemId" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to GetItemCustomSpecification_ByInvItemId." + ex.Message);
            }
        }

        public virtual InvCustomSpec GetCustomSpecification(int id)
        {
            try
            {
                var invCustomSpec = _context.InvCustomSpecs.Find(id);

                return invCustomSpec;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemSpecServices: Unable to GetCustomSpecification" + ex.Message);
                throw new Exception("ItemSpecServices: Unable to GetCustomSpecification." + ex.Message);
            }
        }
    }
}