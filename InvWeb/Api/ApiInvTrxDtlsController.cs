using CoreLib.DTO.Receiving;
using CoreLib.DTO.Releasing;
using CoreLib.Interfaces;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using DevExpress.CodeParser;
using Inventory;
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

        private enum HdrStatus : int
        {
            REQUEST = 1,
            APPROVED = 2,
            CLOSED = 3,
            CANCELLED = 4,
            VERIFIED = 5,
            ACCEPTED =6
        }

        public ApiInvTrxDtlsController(ApplicationDbContext context, ILogger<Controller> logger)
        {
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
                return StatusCode(500, "Add not Successfull");
            }
        }


        [ActionName("AddReleaseTrxDtlItem")]
        [HttpPost]
        public ObjectResult AddReleaseTrxDtlItem(AddReleaseTrxDtlsDTO trxDtls)
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
                invTrxDtl.InvTrxDtlOperatorId = 2;

                _context.InvTrxDtls.Add(invTrxDtl);
                _context.SaveChanges();

                return StatusCode(201, "Add Successfull");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Add not Successfull");
            }
        }


        [ActionName("DeleteTrxDtlItem")]
        [HttpDelete]
        public ObjectResult DeleteTrxDtlItem(int id)
        {
            try
            {

                InvTrxDtl invTrxDtl = itemDtlsServices.GetInvDtlsById(id).First();

                if (invTrxDtl == null)
                {
                    return StatusCode(500, "Unable to find item details");
                }

                _context.InvTrxDtls.Remove(invTrxDtl);
                _context.SaveChanges();

                return StatusCode(201, "Remove Successfull");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Add not Successfull");
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
                return "Err: unable to find HeatLotNo and batchNo.";
            }
        }


        [ActionName("GetInvItemReleasingLotNo")]
        [HttpGet]
        public async Task<string> GetInvItemReleasingLotNo(int id)
        {
            try
            {
                var TrxDetailsLists_Received = await _context.InvTrxDtls
                    .Where(c => c.InvItemId == id && 
                          (c.InvTrxHdr.InvTrxHdrStatusId > (int)HdrStatus.REQUEST && 
                           c.InvTrxHdr.InvTrxHdrStatusId != (int)HdrStatus.CANCELLED) &&
                           c.InvTrxHdr.InvTrxTypeId == 1
                           )
                    .Include(c => c.InvTrxHdr)
                    .Include(c => c.InvUom)
                    .Include(c => c.InvItem)
                    .ThenInclude(c => c.InvCategory)
                    .Include(c => c.InvTrxHdr)
                    .ThenInclude(c => c.InvTrxHdrStatu)
                    .Include(c => c.InvTrxDtlxItemMasters)
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


                if (TrxDetailsLists_Received.Count() == 0)
                {

                    return "List is Empty. no items founds.";
                }

                List<ReceivingTempTrxDetails> invTrxDtls = new List<ReceivingTempTrxDetails>();

                foreach (var item in TrxDetailsLists_Received)
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
                    }

                    trxDetails.Id = item.Id;
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
                 c.LotNo == Lotno && 
                 c.InvTrxDtlOperatorId == ADD)
                .Select(c => c.ItemQty).Sum();

            var invItemTrxList_Released = invTrxDtlsList.Where(c => 
                 c.LotNo == Lotno && 
                 c.InvTrxDtlOperatorId == SUBTRACT)
                .Select(c => c.ItemQty).Sum();

            return invItemTrxList_Received - invItemTrxList_Released;
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
                    invTrxDtl.InvItem.Description
                });

            }
            catch (Exception ex)
            {
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
                return "Unable to GetOriginBrandFromLotNo";
            }


        }




        [ActionName("EditTrxDtlItem")]
        [HttpPost]
        public ObjectResult EditTrxDtlItem(int invDtlsId, int invId, int qty, int uomId)
        {
            try
            {

                InvTrxDtl invTrxDtl = itemDtlsServices.GetInvDtlsById(invDtlsId).FirstOrDefault();

                if (invTrxDtl == null)
                {
                    return StatusCode(500, "Edit not Successfull. Item not found.");
                }

                invTrxDtl.InvItemId = invId;
                invTrxDtl.InvUomId = uomId;
                invTrxDtl.ItemQty = qty;

                //itemDtlsServices.EditInvDtls(invTrxDtl);
                //itemDtlsServices.SaveChangesAsync();

                _context.Attach(invTrxDtl).State = EntityState.Modified;

                _context.SaveChanges();

                return StatusCode(201, "Edit Successfull");
            }
            catch (Exception ex)
            {
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

            //create itemMasters
            InvItemMaster invItemMaster = new InvItemMaster();
            invItemMaster.InvItemId = item.ItemId;
            invItemMaster.InvItemOriginId = item.OriginId;
            invItemMaster.InvItemBrandId = item.BrandId;
            invItemMaster.LotNo = item.LotNo;
            invItemMaster.BatchNo = item.BatchNo;
            invItemMaster.ItemQty = item.Qty;
            invItemMaster.InvUomId = item.UomId;
            invItemMaster.Remarks = item.Remarks;
            invItemMaster.InvStoreAreaId = item.AreaId;

            //save changes
            try
            {

                await invItemMasterServices.CreateInvItemMaster(invItemMaster);
                await invItemMasterServices.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "APIInvTrxDtls/PostReleasingItem: Post Error. Unable to Create invItem Masters.");
            }

            //link to trxDtls and itemMasters
            try
            {
                await invItemMasterServices.CreateItemMasterInvDtlsLink(invItemMaster.Id, item.Id);
                await invItemMasterServices.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "APIInvTrxDtls/PostReleasingItem: Post Error. Unable to Create ItemMaster and InvDtls Link.");
            }

            try
            {
                _context.Attach(invTrxDtl).State = EntityState.Modified;
                await invItemMasterServices.SaveChangesAsync();
            }
            catch (Exception ex)
            {
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
                return StatusCode(500, "PostReceivingItemEdit: Post Error. Unable to Edit invItem Masters.");
            }


            try
            {
                _context.Attach(invTrxDtl).State = EntityState.Modified;
                await invItemMasterServices.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "APIInvTrxDtls/PostReceivingItemEdit: Post Error. Save change on invTrxDtl item.");
            }

            return StatusCode(201, "Update Successfull");
        }


        private class ReceivingTempTrxDetails
        {
            public int Id { get; set; }
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


