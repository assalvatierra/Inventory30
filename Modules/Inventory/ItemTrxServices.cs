using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Modules.Inventory
{
    public class ItemTrxServices : IItemTrxServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        private readonly int TYPE_RELEASING = 2;

        public ItemTrxServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }

        public void CreateInvTrxHdrs(InvTrxHdr invTrxHdr)
        {
            try
            {
                _context.InvTrxHdrs.Add(invTrxHdr);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to CreateInvTrxHdrs :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to CreateInvTrxHdrs :" + ex.Message);
            }
        }

        public void DeleteInvTrxHdrs(InvTrxHdr invTrxHdr)
        {
            try
            {
                _context.InvTrxHdrs.Remove(invTrxHdr);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to DeleteInvTrxHdrs :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to DeleteInvTrxHdrs :" + ex.Message);
            }
        }

        public void EditInvTrxHdrs(InvTrxHdr invTrxHdr)
        {
            try
            {

                _context.Attach(invTrxHdr).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to EditInvTrxHdrs :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to EditInvTrxHdrs :" + ex.Message);
            }
        }

        public async Task SaveChanges()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to SaveChanges :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to SaveChanges :" + ex.Message);
            }
        }

        public bool InvTrxHdrExists(int id)
        {
            return _context.InvTrxHdrs.Any(e => e.Id == id);
        }

        public IQueryable<InvTrxHdr> GetInvTrxHdrs()
        {
            try
            {
                return _context.InvTrxHdrs;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetInvTrxHdrs :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetInvTrxHdrs :" + ex.Message);
            }
        }

        public IQueryable<InvTrxHdr> GetInvTrxHdrsById(int Id)
        {
            try
            {
                return _context.InvTrxHdrs
                    .Include(i => i.InvStore)
                    .Include(i => i.InvTrxHdrStatu)
                    .Include(i => i.InvTrxType)
                    .Where(m => m.Id == Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetInvTrxHdrsById :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetInvTrxHdrsById :" + ex.Message);
            }
        }


        public async Task<InvTrxHdr> GetInvTrxHdrsByIdAsync(int Id)
        {
            try
            {
                var invTrxHdr = await _context.InvTrxHdrs.FindAsync(Id);

                if (invTrxHdr != null)
                {
                    return invTrxHdr;
                }

                return new InvTrxHdr();
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetInvTrxHdrsById :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetInvTrxHdrsById :" + ex.Message);
            }
        }


        public IQueryable<InvTrxHdr> GetTrxHdrsByStoreId_Releasing(int storeId)
        {
            try
            {
                return (IOrderedQueryable<InvTrxHdr>)_context.InvTrxHdrs
                                                             .Include(i => i.InvStore)
                                                             .Include(i => i.InvTrxHdrStatu)
                                                             .Include(i => i.InvTrxDtls)
                                                                .ThenInclude(i => i.InvItem)
                                                                .ThenInclude(i => i.InvUom)
                                                             .Where(i => i.InvTrxTypeId == TYPE_RELEASING && i.InvStoreId == storeId)
                                                             .Include(i => i.InvTrxType);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetInvTrxHdrsByStoreId :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetInvTrxHdrsByStoreId :" + ex.Message);
               
            }
        }

        public IQueryable<InvTrxHdr> GetInvTrxHdrsByStoreId(int storeId, int typeId)
        {
            try
            {
                return (IQueryable<InvTrxHdr>)_context.InvTrxHdrs
                                                             .Include(i => i.InvStore)
                                                             .Include(i => i.InvTrxHdrStatu)
                                                             .Include(i => i.InvTrxDtls)
                                                                .ThenInclude(i => i.InvItem)
                                                                .ThenInclude(i => i.InvUom)
                                                             .Include(i => i.InvTrxType)
                                                             .Where(i => i.InvTrxTypeId == typeId && i.InvStoreId == storeId)
                                                             ;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetInvTrxHdrsByStoreId :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetInvTrxHdrsByStoreId :" + ex.Message);

            }
        }


        public IList<InvTrxHdr> FilterByStatus(IList<InvTrxHdr> invTrxHdrs, string status)
        {
            try
            {

                if (!String.IsNullOrWhiteSpace(status))
                {
                    invTrxHdrs = status switch
                    {
                        "PENDING" => invTrxHdrs.Where(i => i.InvTrxHdrStatusId == 1).ToList(),
                        "ACCEPTED" => invTrxHdrs.Where(i => i.InvTrxHdrStatusId == 2).ToList(),
                        "ALL" => invTrxHdrs.ToList(),
                        _ => invTrxHdrs.Where(i => i.InvTrxHdrStatusId == 1).ToList(),
                    };
                }
                else
                {
                    invTrxHdrs.Where(i => i.InvTrxHdrStatusId == 1).ToList();
                }

                return invTrxHdrs;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to FilterByStatus :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to FilterByStatus :" + ex.Message);

            }
        }


        public IList<InvTrxHdr> FilterByOrder(IList<InvTrxHdr> invTrxHdrs, string order)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(order))
                {
                    invTrxHdrs = order switch
                    {
                        "ASC" => invTrxHdrs.OrderBy(c => c.DtTrx).ToList(),
                        "DESC" => invTrxHdrs.OrderByDescending(c => c.DtTrx).ToList(),
                        _ => invTrxHdrs.OrderBy(c => c.DtTrx).ToList(),
                    };
                }

                return invTrxHdrs;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to FilterByOrder :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to FilterByOrder :" + ex.Message);

            }
        }

        public IQueryable<InvTrxHdrStatus> GetInvTrxHdrStatus()
        {
            try
            {
                return _context.InvTrxHdrStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to FilterByOrder :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to FilterByOrder :" + ex.Message);
            }
        }

        public IQueryable<InvTrxType> GetInvTrxHdrTypes()
        {
            try
            {
                return _context.InvTrxTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to FilterByOrder :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to FilterByOrder :" + ex.Message);
            }
        }

        public IQueryable<InvTrxDtl> GetInvTrxDtlsById(int Id)
        {
            try
            {
                return _context.InvTrxDtls
                            .Include(i => i.InvUom)
                            .Include(i => i.InvItem)
                            .Where(i => i.InvTrxHdrId == Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetInvTrxDtlsById :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetInvTrxDtlsById :" + ex.Message);
            }
        }

        public IQueryable<InvTrxDtl> GetInvTrxDtlsByStoreId(int storeId, int typeId)
        {
            try
            {
                return _context.InvTrxDtls
                            .Include(i => i.InvUom)
                            .Include(i => i.InvItem)
                            .Where(i => i.InvTrxHdr.InvTrxTypeId == typeId && i.InvTrxHdr.InvStoreId == storeId);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetInvTrxDtlsByStoreId :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetInvTrxDtlsByStoreId :" + ex.Message);
            }
        }

        public async Task RemoveTrxDtlsList(int invHdrId)
        {
            try
            {
                var itemList = await _context.InvTrxDtls.Where(i => i.InvTrxHdrId == invHdrId).ToListAsync();
                _context.InvTrxDtls.RemoveRange(itemList);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to RemoveTrxDtlsList :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to RemoveTrxDtlsList :" + ex.Message);
            }
        }


        public int GetInvTrxStoreId(int hdrId)
        {
            try
            {
                var invHdr = this.GetInvTrxHdrsById((int)hdrId).FirstOrDefault();

                if (invHdr == null)
                {
                    return 0;
                }

                return invHdr.InvStoreId;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to RemoveTrxDtlsList :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to RemoveTrxDtlsList :" + ex.Message);
            }
        }
    }
}
