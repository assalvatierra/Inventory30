using InvWeb.Data.Interfaces;
using System;
using System.Threading.Tasks;
using WebDBSchema.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InvWeb.Data.Services
{
    public class ItemSpecServices : IItemSpecServices
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        private readonly int TYPE_RECEIVED = 1;
        private readonly int TYPE_RELEASED = 2;

        private readonly int STATUS_APPROVED = 2;
        private readonly int STATUS_CLOSED = 3;

        public ItemSpecServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }

        public async Task<int> AddItemSpecification(InvItemSpec_Steel invItemSpec)
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

        public async Task<bool> CheckItemHasAnyInvSpec(int invItemId)
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

        public async Task<int> DeleteItemSpecification(InvItemSpec_Steel invItemSpec)
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

        public async Task<int> EditItemSpecification(InvItemSpec_Steel invItemSpec)
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


        public void EditItemSpecificationOnly(InvItemSpec_Steel invItemSpec)
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


        public InvItemSpec_Steel GetItemSpecification(int id)
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

        public List<InvItemSpec_Steel> GetItemSpecificationByInvItemId(int invItemId)
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
    }
}
