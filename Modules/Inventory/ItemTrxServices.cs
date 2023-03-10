using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreLib.DTO.Common.TrxHeader;
using CoreLib.DTO.Receiving;
using CoreLib.DTO.Releasing;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Inventory.DBAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Modules.Inventory
{
    public class ItemTrxServices : IItemTrxServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly DBMasterService dBMaster;

        private readonly int TYPE_RELEASING = 2;

        public ItemTrxServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            dBMaster = new DBMasterService(_context, _logger);

        }

        public virtual void CreateInvTrxHdrs(InvTrxHdr invTrxHdr)
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

        public virtual void DeleteInvTrxHdrs(InvTrxHdr invTrxHdr)
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

        public virtual void EditInvTrxHdrs(InvTrxHdr invTrxHdr)
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

        public virtual async Task SaveChanges()
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

        public virtual bool InvTrxHdrExists(int id)
        {
            return _context.InvTrxHdrs.Any(e => e.Id == id);
        }

        public virtual IQueryable<InvTrxHdr> GetInvTrxHdrs()
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

        public virtual IQueryable<InvTrxHdr> GetInvTrxHdrsById(int Id)
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


        public virtual async Task<InvTrxHdr> GetInvTrxHdrsByIdAsync(int Id)
        {
            try
            {
                //var invTrxHdr = await _context.InvTrxHdrs.FindAsync(Id);

                var invTrxHdr = await _context.InvTrxHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvTrxHdrStatu)
                .Include(i => i.InvTrxType).FirstOrDefaultAsync(m => m.Id == Id);

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


        public virtual IQueryable<InvTrxHdr> GetTrxHdrsByStoreId_Releasing(int storeId)
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

        public virtual IQueryable<InvTrxHdr> GetInvTrxHdrsByStoreId(int storeId, int typeId)
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


        public virtual IList<InvTrxHdr> FilterByStatus(IList<InvTrxHdr> invTrxHdrs, string status)
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


        public virtual IList<InvTrxHdr> FilterByOrder(IList<InvTrxHdr> invTrxHdrs, string order)
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

        public virtual IQueryable<InvTrxHdrStatus> GetInvTrxHdrStatus()
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

        public virtual IQueryable<InvTrxType> GetInvTrxHdrTypes()
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

        public virtual IEnumerable<InvTrxDtl> GetInvTrxDtlsById(int Id)
        {
            try
            {
                var TrxDetailsItems = _context.InvTrxDtls
                            .Include(i => i.InvUom)
                            .Include(i => i.InvItem)
                            .Include(i => i.InvTrxHdr)
                            .Where(i => i.InvTrxHdrId == Id)
                            .ToList();

                if (TrxDetailsItems == null)
                {
                    return new List<InvTrxDtl>();
                }

                return TrxDetailsItems;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetInvTrxDtlsById :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetInvTrxDtlsById :" + ex.Message);
            }
        }

        public virtual IQueryable<InvTrxDtl> GetInvTrxDtlsByStoreId(int storeId, int typeId)
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

        public virtual async Task RemoveTrxDtlsList(int invHdrId)
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


        public virtual int GetInvTrxStoreId(int hdrId)
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

        public virtual async Task<ReleasingIndexModel> GetReleasingIndexModel_OnIndexOnGetAsync(IList<InvTrxHdr> invTrxHdrs, int storeId, int TypeId, string status, bool userIsAdmin)
        {
            try
            {

                invTrxHdrs = await this.GetInvTrxHdrsByStoreId((int)storeId, TYPE_RELEASING).ToListAsync();
                invTrxHdrs = this.FilterByStatus(invTrxHdrs, status);

                //return invTrxHdrs;

                return new ReleasingIndexModel()
                {
                    InvTrxHdrs = invTrxHdrs,
                    Status = status,
                    StoreId = (int)storeId,
                    IsAdmin = userIsAdmin
                };

            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetReleasingIndexModel_OnIndexOnGetAsync :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetReleasingIndexModel_OnIndexOnGetAsync :" + ex.Message);
            }
        }

        public virtual async Task<ReleasingIndexModel> GetReleasingIndexModel_OnIndexOnPostAsync(IList<InvTrxHdr> invTrxHdrs, int storeId, int TypeId, string status, string orderBy, bool userIsAdmin)
        {
            try
            {

                invTrxHdrs = await this.GetInvTrxHdrsByStoreId((int)storeId, TYPE_RELEASING).ToListAsync();
                invTrxHdrs = this.FilterByStatus(invTrxHdrs, status);
                invTrxHdrs = this.FilterByOrder(invTrxHdrs, orderBy);

                //return invTrxHdrs;

                return new ReleasingIndexModel()
                {
                    InvTrxHdrs = invTrxHdrs,
                    Status = status,
                    Order = orderBy,
                    StoreId = (int)storeId,
                    IsAdmin = userIsAdmin
                };


            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetReleasingIndexModel_OnIndexOnPostAsync :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetReleasingIndexModel_OnIndexOnPostAsync :" + ex.Message);
            }
        }

        public virtual ReleasingCreateEditModel GetReleasingCreateModel_OnCreateOnGet(InvTrxHdr invTrxHdr, int storeId, string User, IList<InvStore> invStoreList)
        {
            try
            {

                return new ReleasingCreateEditModel
                {
                    InvTrxHdr = invTrxHdr,
                    InvStoresList = new SelectList(invStoreList, "Id", "StoreName", storeId),
                    InvTrxHdrStatusList = new SelectList(this.GetInvTrxHdrStatus(), "Id", "Status"),
                    InvTrxTypeId = new SelectList(this.GetInvTrxHdrTypes(), "Id", "Type", TYPE_RELEASING),
                    User = User,
                    StoreId = storeId
                };

            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetReleasingCreateModel_OnCreateOnGet :" + ex.Message); 
                throw new Exception("ItemTrxServices: Unable to GetReleasingCreateModel_OnCreateOnGet :" + ex.Message);
                
            }
        }


        public virtual ReleasingCreateEditModel GetReleasingEditModel_OnEditOnGet(InvTrxHdr invTrxHdr, int storeId, string User, IList<InvStore> invStoreList)
        {
            try
            {

                return new ReleasingCreateEditModel
                {
                    InvTrxHdr = invTrxHdr,
                    InvStoresList = new SelectList(invStoreList, "Id", "StoreName", invTrxHdr.InvStoreId),
                    InvTrxHdrStatusList = new SelectList(this.GetInvTrxHdrStatus(), "Id", "Status", invTrxHdr.InvTrxHdrStatusId),
                    InvTrxTypeId = new SelectList(this.GetInvTrxHdrTypes(), "Id", "Type", TYPE_RELEASING),
                    User = User,
                    StoreId = storeId
                };

            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetReleasingEditModel_OnEditOnGet :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetReleasingEditModel_OnEditOnGet :" + ex.Message);

            }
        }




        public virtual async Task DeleteInvTrxHdrs_AndTrxDtlsItems(InvTrxHdr invTrxHdr)
        {
            try
            {
                //remove transactions detail items
                await this.RemoveTrxDtlsList(invTrxHdr.Id);

                //remove header
                this.DeleteInvTrxHdrs(invTrxHdr);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetReleasingCreateModel_OnCreateOnPostAsync :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetReleasingCreateModel_OnCreateOnPostAsync :" + ex.Message);

            }
        }


        public virtual async Task<ReceivingIndexModel> GetReceivingIndexModel_OnIndexOnGetAsync(IList<InvTrxHdr> InvTrxHdrs, int storeId, int TypeId, string status, bool userIsAdmin)
        {
            try
            {
                ReceivingIndexModel receivingIndexModel = new ReceivingIndexModel();

                InvTrxHdrs = await _context.InvTrxHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvTrxHdrStatu)
                .Include(i => i.InvTrxType)
                .Include(i => i.InvTrxDtls)
                    .ThenInclude(i => i.InvItem)
                    .ThenInclude(i => i.InvUom)
                .Where(i => i.InvTrxTypeId == TypeId &&
                              i.InvStoreId == storeId)
                .ToListAsync();

                InvTrxHdrs = this.FilterByStatus(InvTrxHdrs, status);

                receivingIndexModel.InvTrxHdrs = InvTrxHdrs;
                receivingIndexModel.StoreId = storeId;
                receivingIndexModel.Status = status;
                receivingIndexModel.IsAdmin = userIsAdmin;

                return receivingIndexModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetReceivingIndexModel_OnIndexOnGetAsync :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetReceivingIndexModel_OnIndexOnGetAsync :" + ex.Message);

            }
        }


        public virtual async Task<ReceivingIndexModel> GetReceivingIndexModel_OnIndexOnPostAsync(IList<InvTrxHdr> InvTrxHdrs, int storeId, int TypeId, string status, string orderBy, bool userIsAdmin)
        {
            try
            {
                ReceivingIndexModel receivingIndexModel = new ReceivingIndexModel();

                InvTrxHdrs = await _context.InvTrxHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvTrxHdrStatu)
                .Include(i => i.InvTrxType)
                .Include(i => i.InvTrxDtls)
                    .ThenInclude(i => i.InvItem)
                    .ThenInclude(i => i.InvUom)
                .Where(i => i.InvTrxTypeId == TypeId &&
                              i.InvStoreId == storeId)
                .ToListAsync();

                InvTrxHdrs = this.FilterByStatus(InvTrxHdrs, status);
                InvTrxHdrs = this.FilterByOrder(InvTrxHdrs, orderBy);

                receivingIndexModel.InvTrxHdrs = InvTrxHdrs;
                receivingIndexModel.StoreId = storeId;
                receivingIndexModel.Status = status;
                receivingIndexModel.IsAdmin = userIsAdmin;

                return receivingIndexModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetReceivingIndexModel_OnIndexOnGetAsync :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetReceivingIndexModel_OnIndexOnGetAsync :" + ex.Message);

            }
        }


        public virtual ReceivingCreateEditModel GetReceivingCreateModel_OnCreateOnGet(InvTrxHdr invTrxHdr, int storeId, string User)
        {
            try
            {
                return new ReceivingCreateEditModel
                {
                    InvTrxHdr = invTrxHdr,
                    InvStoresList = new SelectList(dBMaster.StoreDb.GetStoreList(), "Id", "StoreName", storeId),
                    InvTrxHdrStatusList = new SelectList(dBMaster.InvTrxHdrStatusDb.GetInvTrxHdrStatus(), "Id", "Status"),
                    InvTrxTypeId = new SelectList(_context.Set<InvTrxType>(), "Id", "Type", 1),
                    User = User,
                    StoreId = storeId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetReleasingCreateModel_OnCreateOnPostAsync :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetReleasingCreateModel_OnCreateOnPostAsync :" + ex.Message);

            }
        }
        public virtual ReceivingCreateEditModel GetReceivingEditModel_OnEditOnGet(InvTrxHdr invTrxHdr, int storeId, string User)
        {
            try
            {
                return new ReceivingCreateEditModel
                {
                    InvTrxHdr = invTrxHdr,
                    InvStoresList = new SelectList(dBMaster.StoreDb.GetStoreList(), "Id", "StoreName", storeId),
                    InvTrxHdrStatusList = new SelectList(dBMaster.InvTrxHdrStatusDb.GetInvTrxHdrStatus(), "Id", "Status"),
                    InvTrxTypeId = new SelectList(_context.Set<InvTrxType>(), "Id", "Type", 1),
                    User = User,
                    StoreId = storeId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetReceivingEditModel_OnEditOnGet :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetReceivingEditModel_OnEditOnGet :" + ex.Message);

            }
        }


        public async Task<TrxHeaderIndexModel> GetTrxHeaderIndexModel_OnGetAsync(IList<InvTrxHdr> invTrxHdrs, int storeId, int typeId, string status, bool userIsAdmin)
        {
            try
            {
                TrxHeaderIndexModel trxIndexModel = new TrxHeaderIndexModel();

                invTrxHdrs = await dBMaster.InvTrxHdrDb.GetInvTrxHdrsByTypeIdAndStoreId(typeId, storeId);

                invTrxHdrs = this.FilterByStatus(invTrxHdrs, status);

                trxIndexModel.InvTrxHdrs = invTrxHdrs;
                trxIndexModel.StoreId = storeId;
                trxIndexModel.Status = status;
                trxIndexModel.IsAdmin = userIsAdmin;

                return trxIndexModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetTrxHeaderIndexModel_OnGetAsync :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetTrxHeaderIndexModel_OnGetAsync :" + ex.Message);

            }
        }
        public async Task<TrxHeaderIndexModel> GetTrxHeaderIndexModel_OnPostAsync(IList<InvTrxHdr> invTrxHdrs, int storeId, int typeId, string status, string orderBy, bool userIsAdmin)
        {
            try
            {
                TrxHeaderIndexModel trxIndexModel = new TrxHeaderIndexModel();

                invTrxHdrs = await dBMaster.InvTrxHdrDb.GetInvTrxHdrsByTypeIdAndStoreId(typeId, storeId);

                invTrxHdrs = this.FilterByStatus(invTrxHdrs, status);
                invTrxHdrs = this.FilterByOrder(invTrxHdrs, orderBy);

                trxIndexModel.InvTrxHdrs = invTrxHdrs;
                trxIndexModel.StoreId = storeId;
                trxIndexModel.Status = status;
                trxIndexModel.IsAdmin = userIsAdmin;

                return trxIndexModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetTrxHeaderIndexModel_OnPostAsync :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetTrxHeaderIndexModel_OnPostAsync :" + ex.Message);

            }
        }

        public TrxHeaderCreateEditModel GetTrxHeaderCreateModel_OnCreateOnGet(InvTrxHdr invTrxHdr, int storeId, string User)
        {
            try
            {
                invTrxHdr = new InvTrxHdr();
                invTrxHdr.InvStoreId = storeId;
                invTrxHdr.InvTrxHdrStatusId = 1;
                invTrxHdr.InvTrxTypeId = 3;

                return new TrxHeaderCreateEditModel
                {
                    InvTrxHdr = invTrxHdr,
                    InvStoresList = new SelectList(dBMaster.StoreDb.GetStoreList(), "Id", "StoreName", storeId),
                    InvTrxHdrStatusList = new SelectList(dBMaster.InvTrxHdrStatusDb.GetInvTrxHdrStatus(), "Id", "Status"),
                    InvTrxTypeId = new SelectList(_context.Set<InvTrxType>(), "Id", "Type", 3),
                    User = User,
                    StoreId = storeId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetTrxHeaderCreateModel_OnCreateOnGet :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetTrxHeaderCreateModel_OnCreateOnGet :" + ex.Message);

            }
        }
        public TrxHeaderCreateEditModel GetTrxHeaderEditModel_OnEditOnGet(InvTrxHdr invTrxHdr, int storeId, string User)
        {
            try
            {
                return new TrxHeaderCreateEditModel
                {
                    InvTrxHdr = invTrxHdr,
                    InvStoresList = new SelectList(dBMaster.StoreDb.GetStoreList(), "Id", "StoreName", storeId),
                    InvTrxHdrStatusList = new SelectList(dBMaster.InvTrxHdrStatusDb.GetInvTrxHdrStatus(), "Id", "Status"),
                    InvTrxTypeId = new SelectList(_context.Set<InvTrxType>(), "Id", "Type", 3),
                    User = User,
                    StoreId = storeId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetTrxHeaderEditModel_OnEditOnGet :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetTrxHeaderEditModel_OnEditOnGet :" + ex.Message);

            }
        }

    }
}
