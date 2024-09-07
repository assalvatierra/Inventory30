using CoreLib.DTO.Adjustment;
using CoreLib.DTO.Receiving;
using CoreLib.DTO.Releasing;
using CoreLib.Interfaces;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using DevExpress.CodeParser;
using Inventory;
using Inventory.DBAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvWeb.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiInvTrxDtlsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IItemTrxServices itemTrxServices;
        private readonly IInvApprovalServices invApprovalServices;
        private readonly IItemDtlsServices itemDtlsServices;
        private readonly IInvItemMasterServices invItemMasterServices;
        private readonly DateServices dateServices;
        private readonly ILogger _logger;

        private enum HdrStatus : int
        {
            REQUEST = 1,
            APPROVED = 2,
            CLOSED = 3,
            CANCELLED = 4,
            VERIFIED = 5,
            ACCEPTED =6
        }
        private enum TRANSACTION_TYPE : int
        {
            RECEIVING = 1,
            RELEASING = 2,
            ADJUSTMENT = 3
        }

        private enum OPERATION : int
        {
            ADD = 1,
            SUBTRACT = 2
        }

        public ApiInvTrxDtlsController(ApplicationDbContext context, ILogger<Controller> logger)
        {
            _logger = logger;
            _context = context;
            dateServices = new DateServices();
            invApprovalServices = new InvApprovalServices(context, logger);
            itemDtlsServices = new ItemDtlsServices(context, logger);
            itemTrxServices = new ItemTrxServices(context, logger);
            invItemMasterServices = new InvItemMasterServices(context, logger);
        }

        // GET: api/<ApiInvTrxDtlsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ApiInvTrxDtlsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ApiInvTrxDtlsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ApiInvTrxDtlsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApiInvTrxDtlsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [ActionName("AddTrxDtlItem")]
        [HttpPost]
        public ObjectResult AddTrxDtlItem(int hdrId,  int invId, int qty, int uomId)
        {
            try
            {

                InvTrxDtl invTrxDtl = new InvTrxDtl();
                invTrxDtl.InvTrxHdrId = hdrId;
                invTrxDtl.InvItemId = invId;
                invTrxDtl.InvUomId = uomId;
                invTrxDtl.ItemQty = qty;
                invTrxDtl.InvTrxDtlOperatorId = 1;

                _context.InvTrxDtls.Add(invTrxDtl);
                _context.SaveChanges();

                return StatusCode(201, "Add Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "Add not Successfull");
            }
        }


        [ActionName("AddTrxDtlItemAdjustment")]
        [HttpPost]
        public ObjectResult AddTrxDtlItemAdjustment(AddItemReleasingDTO addItemReleasingDTO)
        {
            try
            {
                InvTrxDtl invTrxDtl = new InvTrxDtl();
                invTrxDtl.InvTrxHdrId = addItemReleasingDTO.HdrId;
                invTrxDtl.InvItemId = addItemReleasingDTO.InvItemId;
                invTrxDtl.InvUomId = addItemReleasingDTO.UomId;
                invTrxDtl.ItemQty = addItemReleasingDTO.Qty;
                invTrxDtl.LotNo = addItemReleasingDTO.LotNo;
                invTrxDtl.BatchNo = addItemReleasingDTO.BatchNo;
                invTrxDtl.Remarks = addItemReleasingDTO.Remarks;
                invTrxDtl.InvTrxDtlOperatorId = addItemReleasingDTO.OperatorId;

                _context.InvTrxDtls.Add(invTrxDtl);
                _context.SaveChanges();

                CreateInvItemMasterWithLink(addItemReleasingDTO, invTrxDtl.Id);

                return StatusCode(201, "Add Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "Add not Successfull");
            }

        }

        private bool CreateInvItemMasterWithLink(AddItemReleasingDTO addItemReleasingDTO, int trxDtlId)
        {
            try
            {

                InvItemMaster invItemMaster = new InvItemMaster();
                invItemMaster.InvItemId = addItemReleasingDTO.InvItemId;
                invItemMaster.InvItemOriginId = addItemReleasingDTO.OriginId;
                invItemMaster.InvItemBrandId = addItemReleasingDTO.BrandId;
                invItemMaster.InvStoreAreaId = addItemReleasingDTO.AreaId;
                invItemMaster.InvUomId = addItemReleasingDTO.UomId;
                invItemMaster.Remarks = addItemReleasingDTO.Remarks;
                invItemMaster.ItemQty = addItemReleasingDTO.Qty;
                invItemMaster.BatchNo = addItemReleasingDTO.BatchNo;
                invItemMaster.LotNo = addItemReleasingDTO.LotNo;


                invItemMasterServices.CreateInvItemMaster(invItemMaster);
                _context.SaveChanges();

                //Create link
                InvTrxDtlxItemMaster invTrxDtlxItemLink = new InvTrxDtlxItemMaster();

                invTrxDtlxItemLink.InvItemMasterId = invItemMaster.Id;
                invTrxDtlxItemLink.InvTrxDtlId = trxDtlId;

                _context.InvTrxDtlxItemMasters.Add(invTrxDtlxItemLink);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return false;
            }
        }

       


        [ActionName("AddReleaseTrxDtlItem")]
        [HttpPost]
        public async Task<ObjectResult> AddReleaseTrxDtlItem(AddReleaseTrxDtlsDTO trxDtls)
        {
            try
            {

                InvTrxDtl invTrxDtl = new InvTrxDtl();
                invTrxDtl.InvTrxHdrId = trxDtls.hdrId;
                invTrxDtl.InvItemId = trxDtls.invId;
                invTrxDtl.InvUomId = trxDtls.uomId;
                invTrxDtl.ItemQty = trxDtls.qty;
                invTrxDtl.LotNo = trxDtls.lotNo;
                invTrxDtl.BatchNo = trxDtls.batchNo;
                invTrxDtl.InvTrxDtlOperatorId = (int)TRANSACTION_TYPE.RELEASING;

                _context.InvTrxDtls.Add(invTrxDtl);
                await _context.SaveChangesAsync();

                if (trxDtls.itemMasterId != 0 || invTrxDtl.Id != 0)
                {
                    //Bind to Transaction Masters
                    await invItemMasterServices.CreateItemMasterInvDtlsLink(trxDtls.itemMasterId ,invTrxDtl.Id);

                }

                return StatusCode(201, "Add Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "Add not Successfull");
            }
        }


        [ActionName("DeleteTrxDtlItem")]
        [HttpDelete]
        public async Task<ObjectResult> DeleteTrxDtlItem(int id)
        {
            try
            {

                InvTrxDtl invTrxDtl = itemDtlsServices.GetInvDtlsById(id).First();

                if (invTrxDtl == null)
                {
                    return StatusCode(500, "Unable to find item details");
                }

                if (invTrxDtl.InvTrxDtlxItemMasters.Count() > 1)
                {
                    var invTrxInvItemMaster = invTrxDtl.InvTrxDtlxItemMasters.Last();
                    var invItemMasterLinkId = invTrxInvItemMaster.Id;
                    var invItemMasterId = invTrxInvItemMaster.InvItemMasterId;

                    if (invTrxInvItemMaster != null)
                    {
                        if (invTrxDtl.InvTrxHdr.InvTrxTypeId == (int)TRANSACTION_TYPE.RELEASING)
                        {
                            //remove link to itemMaster only
                            await invItemMasterServices.DeleteInvItemMasterLink(invItemMasterLinkId);

                        }
                        else
                        {
                            //remove link to itemMaster
                            await invItemMasterServices.DeleteInvItemMasterLink(invItemMasterLinkId);

                            //remove itemMaster 
                            await invItemMasterServices.DeleteInvItemMaster(invItemMasterId);
                        }


                    }

                }

                _context.InvTrxDtls.Remove(invTrxDtl);
                _context.SaveChanges();

                return StatusCode(201, "Remove Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "Remove not Successfull");
            }
        }


        [ActionName("GetTrxDtlItem")]
        [HttpGet]
        public string GetTrxDtlItem(int id)
        {
            try
            {

                InvTrxDtl invTrxDtl = itemDtlsServices.GetInvDtlsById(id)
                    .Include(i=>i.InvItem)
                    .Include(i=>i.InvUom)
                    .First();

                if (invTrxDtl == null)
                {
                    return "Unable to find item details";
                }

                string Brand = "";
                string Origin = "";
                int OriginId = 0;
                int BrandId = 0;
                string Area = "";
                int AreaId = 0;

                if (!String.IsNullOrEmpty(invTrxDtl.LotNo))
                {

                    InvItemMaster invItemMaster = _context.InvItemMasters.Where(i => i.LotNo == invTrxDtl.LotNo)
                     .Include(c => c.InvItemBrand)
                     .Include(i => i.InvItemOrigin)
                     .Include(i => i.InvStoreArea)
                     .FirstOrDefault();


                    if (invItemMaster != null)
                    {
                        Brand = invItemMaster.InvItemBrand.Name;
                        BrandId = invItemMaster.InvItemBrandId;
                        Origin = invItemMaster.InvItemOrigin.Name;
                        OriginId = invItemMaster.InvItemOriginId;
                        Area = invItemMaster.InvStoreArea.Name;
                        AreaId = invItemMaster.InvStoreAreaId;
                    }
                }


                return JsonConvert.SerializeObject(new
                {
                    invTrxDtl.Id,
                    invTrxDtl.InvItemId,
                    invTrxDtl.InvUomId,
                    invTrxDtl.ItemQty,
                    invTrxDtl.InvItem.Description,
                    invTrxDtl.InvUom.uom,
                    invTrxDtl.LotNo,
                    invTrxDtl.BatchNo,
                    Brand,
                    BrandId,
                    Origin,
                    OriginId,
                    Area,
                    AreaId

                });

            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return "Unable to get Item Details";
            }
        }


        [ActionName("GetTrxDtlItemReleasing")]
        [HttpGet]
        public string GetTrxDtlItemReleasing(int id)
        {
            try
            {

                InvTrxDtl invTrxDtl = itemDtlsServices.GetInvDtlsById(id)
                    .Include(i => i.InvItem)
                    .Include(i => i.InvUom)
                    .First();

                if (invTrxDtl == null)
                {
                    return "Unable to find item details";
                }

                string Brand = "";
                string Origin = "";
                int OriginId = 0;
                int BrandId = 0;
                string Area = "";
                int AreaId = 0;

                if (!String.IsNullOrEmpty(invTrxDtl.LotNo))
                {

                    InvItemMaster invItemMaster = _context.InvItemMasters.Where(i => i.LotNo == invTrxDtl.LotNo)
                     .Include(c => c.InvItemBrand)
                     .Include(i => i.InvItemOrigin)
                     .Include(i => i.InvStoreArea)
                     .FirstOrDefault();


                    if (invItemMaster != null)
                    {
                        Brand = invItemMaster.InvItemBrand.Name;
                        BrandId = invItemMaster.InvItemBrandId;
                        Origin = invItemMaster.InvItemOrigin.Name;
                        OriginId = invItemMaster.InvItemOriginId;
                        Area = invItemMaster.InvStoreArea.Name;
                        AreaId = invItemMaster.InvStoreAreaId;
                    }
                }


                return JsonConvert.SerializeObject(new
                {
                    invTrxDtl.Id,
                    invTrxDtl.InvItemId,
                    invTrxDtl.InvUomId,
                    invTrxDtl.ItemQty,
                    invTrxDtl.InvItem.Description,
                    invTrxDtl.InvUom.uom,
                    invTrxDtl.LotNo,
                    invTrxDtl.BatchNo,
                    Brand,
                    BrandId,
                    Origin,
                    OriginId,
                    Area,
                    AreaId

                });

            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return "Unable to get Item Details";
            }
        }


        [ActionName("GetInvItemLotHeatNo")]
        [HttpGet]
        public string GetInvItemLotHeatNo(int id)
        {
            try
            {
                var trxDtls = _context.InvTrxDtls.Where(c => c.Id == id).Include(c=>c.InvTrxHdr).FirstOrDefault();

                var trxHdr = _context.InvTrxHdrs.Where(c=>c.Id == trxDtls.InvTrxHdrId)
                    .Include(c=>c.InvTrxDtls)
                    .ThenInclude(c => c.InvTrxDtlxItemMasters)
                    .ThenInclude(c => c.InvItemMaster)
                    .FirstOrDefault();

                var trxDtlsCount = trxHdr.InvTrxDtls.Where(c => c.InvTrxDtlxItemMasters.Count() > 0).ToList().Count();

                if (trxDtlsCount == 0)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        LotNo = "",
                        BatchNo = "",
                        InvItemOriginId = 1,
                        InvItemBrandId = 1
                    });
                }


                var similarTrxDetails = trxHdr.InvTrxDtls.Where(c => c.InvTrxDtlxItemMasters.Count() > 0).Last();

                //List<InvTrxDtlxItemMaster> invTrxDtlxItemMasters = _context.InvTrxDtlxItemMasters
                //    .Where(c => c.InvTrxDtlId == id)
                //    .Include(c=>c.InvItemMaster)
                //    .ToList();

                if (similarTrxDetails == null)
                {
             
                    return JsonConvert.SerializeObject(new
                    {
                        LotNo = "",
                        BatchNo = "",
                        InvItemOriginId = 1,
                        InvItemBrandId = 1
                    });
                }

                var similarItemMaster = similarTrxDetails.InvTrxDtlxItemMasters.LastOrDefault();

                InvItemMaster invTrxDtl = similarItemMaster.InvItemMaster;

                if (invTrxDtl == null)
                {
                    return "Unable to find item details";
                }

                return JsonConvert.SerializeObject(new
                {
                    invTrxDtl.Id,
                    invTrxDtl.LotNo,
                    invTrxDtl.BatchNo,
                    invTrxDtl.InvItemOriginId,
                    invTrxDtl.InvItemBrandId,
                });

            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return "Err: unable to find HeatLotNo and batchNo.";
            }
        }


        [ActionName("GetInvItemReceivedTrx")]
        [HttpGet]
        public async Task<string> GetInvItemReceivedTrx(int id)
        {
            try
            {
                var TrxDetailsLists = await _context.InvTrxDtls
                    .Where(c => c.InvItemId == id &&
                          (c.InvTrxHdr.InvTrxHdrStatusId > (int)HdrStatus.REQUEST &&
                           c.InvTrxHdr.InvTrxHdrStatusId != (int)HdrStatus.CANCELLED))
                    .Include(c => c.InvTrxHdr)
                    .Include(c=>c.InvUom)
                    .Include(c=>c.InvItem)
                    .ThenInclude(c => c.InvCategory)
                    .Include(c => c.InvTrxHdr)
                    .ThenInclude(c => c.InvTrxHdrStatu)
                    .Include(c=>c.InvTrxDtlxItemMasters)
                    .ThenInclude(c => c.InvItemMaster)
                    .Include(c => c.InvTrxDtlxItemMasters)
                    .ThenInclude(c => c.InvItemMaster.InvItemOrigin)
                    .Include(c => c.InvTrxDtlxItemMasters)
                    .ThenInclude(c => c.InvItemMaster.InvItemBrand)
                    .ToListAsync();

                var invItemTrxList_ByItem = await _context.InvTrxDtls.Where(c =>
                     c.InvItemId == id &&
                    (c.InvTrxHdr.InvTrxHdrStatusId == (int)HdrStatus.APPROVED ||
                     c.InvTrxHdr.InvTrxHdrStatusId == (int)HdrStatus.CLOSED))
                    .Include(c => c.InvTrxHdr)
                    .Include(c => c.InvTrxHdr)
                    .ThenInclude(c => c.InvTrxHdrStatu).ToListAsync();


                if (TrxDetailsLists.Count() == 0)
                {

                    return  "List is Empty. no items founds.";
                }

                List<ReceivingTempTrxDetails> invTrxDtls = new List<ReceivingTempTrxDetails>();

                foreach (var item in TrxDetailsLists)
                {
                    ReceivingTempTrxDetails trxDetails = new ReceivingTempTrxDetails();
                    var BatchNo = "";
                    var LotNo = "";
                    var Origin = "";
                    var Brand = "";

                    if (item.InvTrxDtlxItemMasters.Count() > 0)
                    {
                        BatchNo = item.InvTrxDtlxItemMasters.First().InvItemMaster.BatchNo;
                        LotNo = item.InvTrxDtlxItemMasters.First().InvItemMaster.LotNo;
                        Origin = item.InvTrxDtlxItemMasters.First().InvItemMaster.InvItemBrand.Name;
                        Brand = item.InvTrxDtlxItemMasters.First().InvItemMaster.InvItemOrigin.Name;
                    }

                    trxDetails.Id = item.Id;
                    trxDetails.ItemId = item.InvItemId;
                    trxDetails.Description = item.InvItem.InvCategory.Description + " - " + item.InvItem.Description;
                    trxDetails.Brand = Brand;
                    trxDetails.BatchNo = BatchNo;
                    trxDetails.LotNo = LotNo;
                    trxDetails.Code = item.InvItem.Code;
                    trxDetails.Date = item.InvTrxHdr.DtTrx.ToShortDateString();
                    trxDetails.Origin = Origin;
                    trxDetails.Qty = item.ItemQty;
                    trxDetails.OnStockQty = GetTotalBalanceOfItemFromLotNo(item.InvItemId, LotNo, invItemTrxList_ByItem);
                    trxDetails.Uom = item.InvUom.uom;
                    trxDetails.Status = item.InvTrxHdr.InvTrxHdrStatu.Status;
                    

                    invTrxDtls.Add(trxDetails);
                }

                if (invTrxDtls == null)
                {
                    return "Unable to find item details";
                }

                return JsonConvert.SerializeObject( invTrxDtls );


            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return "Err: unable to find HeatLotNo and batchNo.";
            }
        }


        [ActionName("GetInvItemReleasingLotNo")]
        [HttpGet]
        public async Task<string> GetInvItemReleasingLotNo(int id)
        {
            try
            {
                var invItemMasterListByItem_Received =  await _context.InvItemMasters
                        .Where(c => c.InvItemId == id && c.InvTrxDtlxItemMasters.Count > 0 
                        && c.InvTrxDtlxItemMasters.FirstOrDefault().InvTrxDtl.InvTrxHdr.InvTrxTypeId == (int)TRANSACTION_TYPE.RECEIVING
                        && (c.InvTrxDtlxItemMasters.FirstOrDefault().InvTrxDtl.InvTrxHdr.InvTrxHdrStatusId == (int)HdrStatus.APPROVED
                        || c.InvTrxDtlxItemMasters.FirstOrDefault().InvTrxDtl.InvTrxHdr.InvTrxHdrStatusId == (int)HdrStatus.CLOSED))
                    .Include(c=>c.InvItemBrand)
                    .Include(c => c.InvItemOrigin)
                    .Include(c => c.InvStoreArea)
                    .Include(c => c.InvTrxDtlxItemMasters)
                        .ThenInclude(c => c.InvTrxDtl)
                    .Include(c => c.InvTrxDtlxItemMasters)
                        .ThenInclude(c => c.InvTrxDtl)
                        .ThenInclude(c => c.InvTrxDtlOperator)
                    .Include(c => c.InvTrxDtlxItemMasters)
                        .ThenInclude(c => c.InvTrxDtl)
                        .ThenInclude(c => c.InvTrxHdr.InvTrxHdrStatu)
                    .Include(c => c.InvItem)
                    .Include(c => c.InvItem)
                        .ThenInclude(c=>c.InvCategory)
                    .Include(c => c.InvItem)
                        .ThenInclude(c => c.InvUom)
                    .ToListAsync();

                if (invItemMasterListByItem_Received.Count() == 0)
                {

                    return "List is Empty. no items founds.";
                }

                List<ReceivingTempTrxDetails> invTrxDtls = new List<ReceivingTempTrxDetails>();

                foreach (var item in invItemMasterListByItem_Received)
                {
                    ReceivingTempTrxDetails trxDetails = new ReceivingTempTrxDetails();
                    var BatchNo = "";
                    var LotNo = "";
                    var Origin = "";
                    var Brand = "";

                    if (item.InvTrxDtlxItemMasters.Count() > 0)
                    {
                        BatchNo = item.InvTrxDtlxItemMasters.First().InvItemMaster.BatchNo;
                        LotNo = item.InvTrxDtlxItemMasters.First().InvItemMaster.LotNo;
                        Origin = item.InvTrxDtlxItemMasters.First().InvItemMaster.InvItemBrand.Name;
                        Brand = item.InvTrxDtlxItemMasters.First().InvItemMaster.InvItemOrigin.Name;

                        //use itemMasterLotno
                        item.LotNo = item.InvTrxDtlxItemMasters.First().InvItemMaster.LotNo;

                        trxDetails.Date = item.InvTrxDtlxItemMasters.First().InvTrxDtl.InvTrxHdr.DtTrx.ToShortDateString();
                        trxDetails.Status = item.InvTrxDtlxItemMasters.First().InvTrxDtl.InvTrxHdr.InvTrxHdrStatu.Status;
                    }

                    trxDetails.Id = item.Id;
                    trxDetails.ItemId = item.InvItemId;
                    trxDetails.InvItemMasterId = item.Id;
                    trxDetails.Description = item.InvItem.InvCategory.Description + " - " + item.InvItem.Description;
                    trxDetails.Brand = Brand;
                    trxDetails.BatchNo = BatchNo;
                    trxDetails.LotNo = LotNo;
                    trxDetails.Origin = Origin;
                    trxDetails.Code = item.InvItem.Code;
                    trxDetails.Qty = item.ItemQty;
                    trxDetails.Uom = item.InvUom.uom;

                    trxDetails.OnStockQty = GetTotalItemStockFromItemMaster(item.Id);

                    if (!String.IsNullOrEmpty(LotNo))
                    {
                        invTrxDtls.Add(trxDetails);
                    }
                }

                if (invTrxDtls == null)
                {
                    return "Unable to find item details";
                }

                return JsonConvert.SerializeObject(invTrxDtls);


            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return "Err: unable to find HeatLotNo and batchNo.";
            }
        }

        private int GetTotalBalanceOfItemFromLotNo(int invItemId, string Lotno, List<InvTrxDtl> invTrxDtlsList)
        {
            if (string.IsNullOrEmpty(Lotno) || Lotno == "")
            {
                return 0;
            }

            int ADD = 1;
            int SUBTRACT = 2;

            var invItemTrxList_Received = invTrxDtlsList.Where(c => 
                 c.LotNo == Lotno && c.InvItemId == invItemId &&
                 c.InvTrxDtlOperatorId == ADD)
                .Select(c => c.ItemQty).Sum();

            var invItemTrxList_Released = invTrxDtlsList.Where(c => 
                 c.LotNo == Lotno && c.InvItemId == invItemId &&
                 c.InvTrxDtlOperatorId == SUBTRACT)
                .Select(c => c.ItemQty).Sum();

            return invItemTrxList_Received - invItemTrxList_Released;
        }


        private int GetTotalItemStockFromItemMaster(int itemMasterId)
        {

            var InvTrxDtlxItemMasters = invItemMasterServices.GetInvTrxDtlxItemMaster_byId(itemMasterId);

            //int invItem_Received = InvTrxDtlxItemMasters.Where( i => i.InvTrxDtl.InvTrxDtlOperatorId == (int)OPERATION.ADD )
            //                                            .Sum( i => i.InvItemMaster.ItemQty);
            //int invItem_Released = InvTrxDtlxItemMasters.Where(i => i.InvTrxDtl.InvTrxDtlOperatorId == (int)OPERATION.SUBTRACT )
            //                                            .Sum(i => i.InvTrxDtl.ItemQty);
            int invItem_Received = 0;
            int invItem_Released = 0;

            foreach (var item in InvTrxDtlxItemMasters)
            {
                if (item.InvTrxDtl.InvTrxDtlOperatorId == (int)OPERATION.ADD)
                {
                    invItem_Received += item.InvItemMaster.ItemQty;
                }

                if (item.InvTrxDtl.InvTrxDtlOperatorId == (int)OPERATION.SUBTRACT)
                {
                    invItem_Released += item.InvTrxDtl.ItemQty;
                }
            }
            return invItem_Received - invItem_Released;
        }


        [ActionName("GetItemDetails")]
        [HttpGet]
        public string GetItemDetails(int id)
        {
            try
            {
                InvTrxDtl invTrxDtl = itemDtlsServices.GetInvDtlsById(id).FirstOrDefault();

                if (invTrxDtl == null)
                {
                    return "Unable to find item details";
                }

              
                return JsonConvert.SerializeObject(new {
                    invTrxDtl.Id,
                    invTrxDtl.InvItemId,
                    invTrxDtl.InvUomId,
                    invTrxDtl.ItemQty,
                    invTrxDtl.InvUom.uom,
                    invTrxDtl.LotNo,
                    invTrxDtl.BatchNo,
                    invTrxDtl.InvItem.Description,
                    invTrxDtl.Remarks,
                    invTrxDtl.InvTrxDtlOperatorId
                });

            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return "Add not GetItemDetails";
            }
        }


        [ActionName("GetItemUom")]
        [HttpGet]
        public string GetItemUom(int id)
        {
            try
            {
                InvItem invItem = _context.InvItems.Where(i=>i.Id == id)
                    .Include(c=>c.InvUom)
                    .Include(i=>i.InvCategory)
                    .FirstOrDefault();


                if (invItem == null)
                {
                    return "Unable to find item details";
                }


                return JsonConvert.SerializeObject(new
                {
                    invItem.Id,
                    invItem.InvUomId,
                    invItem.InvUom.uom,
                    invItem.Description
                });

            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return "Add not GetItemDetails";
            }
        }

        [ActionName("GetOriginBrandFromLotNo")]
        [HttpGet]
        public string GetOriginBrandFromLotNo(string lotno)
        {
            try
            {

                InvItemMaster invItemMaster = _context.InvItemMasters.Where(i => i.LotNo == lotno)
                    .Include(c => c.InvItemBrand)
                    .Include(i => i.InvItemOrigin)
                    .FirstOrDefault();

                string brand = "";
                string origin = "";

                if (invItemMaster != null)
                {
                    brand = invItemMaster.InvItemBrand.Name;
                    origin = invItemMaster.InvItemOrigin.Name;
                }

                return JsonConvert.SerializeObject(new
                {
                    lotno,
                    brand,
                    origin
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return "Unable to GetOriginBrandFromLotNo";
            }


        }


        [ActionName("GetItemMasterDetails")]
        [HttpGet]
        public string GetItemMasterDetails(int Id)
        {
            try
            {

                InvTrxDtl invTrxDtl = _context.InvTrxDtls.Where(c=>c.Id == Id)
                    .Include(c => c.InvTrxDtlxItemMasters)
                        .ThenInclude(c => c.InvItemMaster)
                    .Include(c => c.InvTrxDtlxItemMasters)
                        .ThenInclude(c=>c.InvItemMaster)
                            .ThenInclude(c => c.InvItemBrand)
                    .Include(c=>c.InvTrxDtlxItemMasters)
                        .ThenInclude(c => c.InvItemMaster)
                            .ThenInclude(c => c.InvItemOrigin)
                    .Include(c => c.InvTrxDtlxItemMasters)
                        .ThenInclude(c => c.InvItemMaster)
                            .ThenInclude(c => c.InvStoreArea)
                    .FirstOrDefault();

                if (invTrxDtl == null)
                {
                    return "Unable to find invTrxDtl";
                }


                InvItemMaster invItemMaster = invTrxDtl.InvTrxDtlxItemMasters.FirstOrDefault().InvItemMaster;

                return JsonConvert.SerializeObject(new
                {
                    invItemMaster.InvItemBrandId,
                    invItemMaster.InvItemOriginId,
                    invItemMaster.InvStoreAreaId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return "Unable to GetOriginBrandFromLotNo";
            }


        }



        [ActionName("EditTrxDtlItem")]
        [HttpPost]
        public ObjectResult EditTrxDtlItem(EditItemReleasingDTO editItem)
        {
            try
            {

                InvTrxDtl invTrxDtl = itemDtlsServices.GetInvDtlsById(editItem.InvTrxDetailsId).Include(c=>c.InvTrxDtlxItemMasters).FirstOrDefault();

                if (invTrxDtl == null)
                {
                    return StatusCode(500, "Edit not Successfull. Item not found.");
                }

                invTrxDtl.InvItemId = editItem.InvItemId;
                invTrxDtl.InvUomId = editItem.UomId;
                invTrxDtl.ItemQty = editItem.Qty;
                invTrxDtl.LotNo = editItem.LotNo;
                invTrxDtl.BatchNo = editItem.BatchNo;
                invTrxDtl.Remarks = editItem.Remarks;

                InvItemMaster invItemMaster = invTrxDtl.InvTrxDtlxItemMasters.FirstOrDefault().InvItemMaster;
                
                if (invItemMaster == null)
                {
                    return StatusCode(500, "Edit not Successfull. invItemMaster not found.");
                }


                invItemMaster.InvStoreAreaId = editItem.AreaId;
                invItemMaster.InvItemBrandId = editItem.BrandId;
                invItemMaster.InvItemOriginId = editItem.OriginId;
                invItemMaster.ItemQty = editItem.Qty;
                invItemMaster.LotNo = editItem.LotNo;
                invItemMaster.BatchNo = editItem.BatchNo;
                invItemMaster.Remarks = editItem.Remarks;

                _context.Attach(invTrxDtl).State = EntityState.Modified;

                _context.Attach(invItemMaster).State = EntityState.Modified;


                _context.SaveChanges();

                return StatusCode(201, "Edit Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "Edit not Successfull");
            }
        }


        [ActionName("EditReleaseTrxDtlItem")]
        [HttpPost]
        public ObjectResult EditReleaseTrxDtlItem(int invDtlsId, int invId, int qty, int uomId, string lotNo, string batchNo)
        {
            try
            {
                if (invDtlsId == 0)
                {
                    return StatusCode(500, "EditReleaseTrxDtlItem: InvDtlsId is empty");
                }

                InvTrxDtl invTrxDtl = itemDtlsServices.GetInvDtlsById(invDtlsId).FirstOrDefault();

                if (invTrxDtl == null)
                {
                    return StatusCode(500, "Edit not Successfull. Item not found.");
                }

                invTrxDtl.InvItemId = invId;
                invTrxDtl.InvUomId = uomId;
                invTrxDtl.ItemQty = qty;
                invTrxDtl.LotNo = lotNo;
                invTrxDtl.BatchNo = batchNo;

                _context.Attach(invTrxDtl).State = EntityState.Modified;

                _context.SaveChanges();

                return StatusCode(201, "Edit Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "Edit not Successfull");
            }
        }



        [ActionName("EditHdrRemarks")]
        [HttpPost]
        public ObjectResult EditHdrRemarks(int hdrId, string remarks)
        {
            try
            {

                InvTrxHdr invTrxHdr = itemTrxServices.GetInvTrxHdrsById(hdrId).FirstOrDefault();

                if (invTrxHdr == null)
                {
                    return StatusCode(500, "Edit not Successfull. Header Details not found.");
                }

                invTrxHdr.Remarks = remarks;

                //itemDtlsServices.EditInvDtls(invTrxDtl);
                //itemDtlsServices.SaveChangesAsync();

                _context.Attach(invTrxHdr).State = EntityState.Modified;

                _context.SaveChanges();

                return StatusCode(201, "Edit Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "Edit not Successfull");
            }
        }


        [ActionName("PostReceivingItem")]
        [HttpPost]
        public async Task<ObjectResult> PostReceivingItem(ReceivingTrxItemApiModel item)
        {
            if (item == null)
            {
                return StatusCode(500, "Post Error. Header Details not found.");
            }
            InvTrxDtl invTrxDtl = _context.InvTrxDtls.Find(item.Id);

            if (invTrxDtl == null)
            {
                return StatusCode(500, "Post Error. Header Details not found.");
            }

            //update invTrxDtls
            invTrxDtl.LotNo = item.LotNo;
            invTrxDtl.BatchNo = item.BatchNo;


            //create itemMasters
            InvItemMaster invItemMaster = new InvItemMaster();
            invItemMaster.InvItemId = item.ItemId;
            invItemMaster.InvItemOriginId = item.OriginId;
            invItemMaster.InvItemBrandId = item.BrandId;
            invItemMaster.LotNo = item.LotNo;
            invItemMaster.BatchNo = item.BatchNo;
            invItemMaster.ItemQty = item.Qty; // For Release, negative number
            invItemMaster.InvUomId = item.UomId;
            invItemMaster.InvStoreAreaId = item.AreaId;
            //invItemMaster.Remarks = item.Remarks;


            //save changes
            try
            {
                itemDtlsServices.EditInvDtls(invTrxDtl);


                await invItemMasterServices.CreateInvItemMaster(invItemMaster);
                await invItemMasterServices.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "APIInvTrxDtls: Post Error. Unable to Create invItem Masters.");
            }

            //link to trxDtls and itemMasters
            try
            {
                await invItemMasterServices.CreateItemMasterInvDtlsLink(invItemMaster.Id, item.Id);
                await invItemMasterServices.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "APIInvTrxDtls: Post Error. Unable to Create ItemMaster and InvDtls Link.");
            }

            return StatusCode(201, "Update Successfull");
        }


        [ActionName("PostReleasingItem")]
        [HttpPost]
        public async Task<ObjectResult> PostReleasingItem(ReceivingTrxItemApiModel item)
        {
            if (item == null)
            {
                return StatusCode(500, "Post Error. Header Details not found.");
            }

            InvTrxDtl invTrxDtl = _context.InvTrxDtls.Find(item.Id);
            invTrxDtl.LotNo = item.LotNo;
            invTrxDtl.BatchNo = item.BatchNo;

            var invitemMasterId = 0; // TODO: set invItemMasterId

            if (_context.InvItemMasters.Where(c=>c.Id == invitemMasterId).FirstOrDefault() == null)
            {
                return StatusCode(500, "Post Error. Header Details not found.");
            }

            //link to trxDtls and itemMasters
            try
            {
                await invItemMasterServices.CreateItemMasterInvDtlsLink(invitemMasterId, item.Id);
                await invItemMasterServices.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "APIInvTrxDtls/PostReleasingItem: Post Error. Unable to Create ItemMaster and InvDtls Link.");
            }

            try
            {
                _context.Attach(invTrxDtl).State = EntityState.Modified;
                await invItemMasterServices.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "APIInvTrxDtls/PostReleasingItem: Post Error. Save change on invTrxDtl item.");
            }

            return StatusCode(201, "Update Successfull");
        }






        //GetItemMaster

        [ActionName("GetItemMaster")]
        [HttpGet]
        public string GetItemMaster(int id)
        {
            try
            {

                InvItemMaster invItemMaster = _context.InvItemMasters
                    .Where(i=>i.Id == id)
                    .Include(i=>i.InvUom)
                    .Include(i=>i.InvItem)
                    .FirstOrDefault();

                if (invItemMaster == null)
                {
                    return "Unable to find item details";
                }


                return JsonConvert.SerializeObject(new
                {
                    invItemMaster.Id,
                    invItemMaster.InvUomId,
                    invItemMaster.InvUom.uom,
                    invItemMaster.InvItemId,
                    invItemMaster.InvItem.Description,
                    invItemMaster.LotNo,
                    invItemMaster.BatchNo,
                    invItemMaster.InvItemBrandId,
                    invItemMaster.InvItemOriginId,
                    invItemMaster.ItemQty,
                    invItemMaster.InvStoreAreaId,
                    invItemMaster.Remarks
                });

            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return "Get Item Details not Successfull";
            }
        }



        [ActionName("PostReceivingItemEdit")]
        [HttpPost]
        public async Task<ObjectResult> PostReceivingItemEdit(ReleasingTrxItemApiModel item)
        {
            if (item == null)
            {
                return StatusCode(500, "Post Error. Header Details not found.");
            }

            InvTrxDtl invTrxDtl = _context.InvTrxDtls.Find(item.TrxId);
            if (invTrxDtl == null)
            {
                return StatusCode(500, "Post Error. invTrxDtl Details not found.");

            }

            invTrxDtl.LotNo = item.LotNo;
            invTrxDtl.BatchNo = item.BatchNo;


            //create itemMasters
            InvItemMaster invItemMaster = _context.InvItemMasters.Find(item.Id);
           
            if (invItemMaster == null)
            {
                return StatusCode(500, "Post Error. invItemMaster Details not found.");
          
            }

            invItemMaster.InvItemId = item.ItemId;
            invItemMaster.InvItemOriginId = item.OriginId;
            invItemMaster.InvItemBrandId = item.BrandId;
            invItemMaster.LotNo = item.LotNo;
            invItemMaster.BatchNo = item.BatchNo;
            invItemMaster.ItemQty = item.Qty;
            invItemMaster.InvUomId = item.UomId;
            invItemMaster.InvStoreAreaId = item.AreaId;
            invItemMaster.Remarks = item.Remarks;
            invItemMaster.InvItemOriginId = item.OriginId;
            invItemMaster.InvItemBrandId = item.BrandId;

            //save changes
            try
            {
                //invItemMasterServices.EditInvItemMaster(invItemMaster);
                //invItemMasterServices.SaveChangesAsync();

                _context.Attach(invItemMaster).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "PostReceivingItemEdit: Post Error. Unable to Edit invItem Masters.");
            }


            try
            {
                _context.Attach(invTrxDtl).State = EntityState.Modified;
                await invItemMasterServices.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ApiInvTrxDtlsController: " + ex.Message);
                return StatusCode(500, "APIInvTrxDtls/PostReceivingItemEdit: Post Error. Save change on invTrxDtl item.");
            }

            return StatusCode(201, "Update Successfull");
        }


        private class ReceivingTempTrxDetails
        {
            public int Id { get; set; }
            public int ItemId { get; set; }
            public int InvItemMasterId { get; set; }
            public string Description { get; set; }
            public string Code { get; set; }
            public string Origin { get; set; }
            public string Brand { get; set; }
            public string Date { get; set; }
            public string LotNo { get; set; }
            public string BatchNo { get; set; }
            public decimal Qty { get; set; }
            public decimal OnStockQty { get; set; }
            public string Uom { get; set; }
            public string Status { get; set; }
        }


    }

}


