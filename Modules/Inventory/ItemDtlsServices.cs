﻿using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Inventory
{
    public class ItemDtlsServices : IItemDtlsServices
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        private readonly int TYPE_RELEASING = 2;

        public ItemDtlsServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }

        public void CreateInvDtls(InvTrxDtl invTrxDtl)
        {
            try
            {
                _context.InvTrxDtls.Add(invTrxDtl);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Unable to CreateInvDtls :" + ex.Message);
                throw new Exception("ItemDtlsServices: Unable to CreateInvDtls :" + ex.Message);
            }
        }

        public void DeleteInvDtls(InvTrxDtl invTrxDtl)
        {
            try
            {
                _context.InvTrxDtls.Remove(invTrxDtl);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Unable to DeleteInvDtls :" + ex.Message);
                throw new Exception("ItemDtlsServices: Unable to DeleteInvDtls :" + ex.Message);
            }
        }

        public void EditInvDtls(InvTrxDtl invTrxDtl)
        {
            try
            {

                _context.Attach(invTrxDtl).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Unable to EditInvDtls :" + ex.Message);
                throw new Exception("ItemDtlsServices: Unable to EditInvDtls :" + ex.Message);
            }
        }

        public IQueryable<InvTrxDtl> GetInvDtlsById(int Id)
        {
            try
            {
                return _context.InvTrxDtls
                 .Include(i => i.InvItem)
                 .Include(i => i.InvTrxHdr)
                 .Include(i => i.InvUom)
                 .Where(i => i.Id == Id);
            }
            catch (Exception ex)
            {
                    _logger.LogError("ItemDtlsServices: Unable to GetInvDtlsById :" + ex.Message);
                    throw new Exception("ItemDtlsServices: Unable to GetInvDtlsById :" + ex.Message);
                
            }
        }

        public async Task<InvTrxDtl> GetInvDtlsByIdAsync(int Id)
        {
            return await _context.InvTrxDtls.FindAsync(Id);
        }

        public async Task<InvTrxDtl> GetInvDtlsByIdOnEdit(int Id)
        {
            var invTrx = await _context.InvTrxDtls
                .Include(i => i.InvItem)
                .ThenInclude(i => i.InvWarningLevels)
                .ThenInclude(i => i.InvWarningType)
                .Include(i => i.InvTrxHdr)
                .Include(i => i.InvUom)
                .FirstOrDefaultAsync(m => m.Id == Id);

            if (invTrx == null)
            {
                return new InvTrxDtl();
            }

            return invTrx;
        }

        public IQueryable<InvTrxHdr> GetInvDtlsByStoreId(int storeId, int typeId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<InvTrxDtlOperator> GetInvTrxDtlOperators()
        {
            try
            {
                return _context.InvTrxDtlOperators;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Error on GetInvTrxDtlOperators :" + ex.Message);
                throw new Exception("ItemDtlsServices: Error on  GetInvTrxDtlOperators :" + ex.Message);

            }
        }

        public bool InvTrxDtlsExists(int id)
        {
            return _context.InvTrxDtls.Any(e => e.Id == id);
        }

        public async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }

        IQueryable<InvTrxDtl> IItemDtlsServices.GetInvDtlsByStoreId(int storeId, int typeId)
        {
            throw new NotImplementedException();
        }
    }
}