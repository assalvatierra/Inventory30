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

                return JsonConvert.SerializeObject(new
                {
                    invTrxDtl.Id,
                    invTrxDtl.InvItemId,
                    invTrxDtl.InvUomId,
                    invTrxDtl.ItemQty,
                    invTrxDtl.InvItem.Description,
                    invTrxDtl.InvUom.uom,
                    invTrxDtl.LotNo,
                    invTrxDtl.BatchNo
                });

            }
            catch (Exception ex)
            {
                return "Add not Successfull";
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
        public string GetInvItemOnReceivedTrx(int id)
        {
            try
            {
                var TrxDetailsLists = _context.InvTrxDtls.Where(c => c.InvItemId == id && c.InvTrxHdr.InvTrxHdrStatusId > 1)
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
                    .ToList();


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


        [ActionName("GetItemDetails")]
        [HttpGet]
        public string GetItemDetails(int id)
        {
            try
            {

                InvTrxDtl invTrxDtl = itemDtlsServices.GetInvDtlsById(id).First();

                if (invTrxDtl == null)
                {
                    return "Unable to find item details";
                }



                return JsonConvert.SerializeObject(new {
                     invTrxDtl.Id,
                     invTrxDtl.InvItemId,
                    invTrxDtl.InvUomId,
                    invTrxDtl.ItemQty,
                    invTrxDtl.InvItem.InvUom.uom,
                    invTrxDtl.LotNo
                });

            }
            catch (Exception ex)
            {
                return "Add not Successfull";
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
        public ObjectResult EditReleaseTrxDtlItem(int invDtlsId, int invId, int qty, int uomId, int lotNo)
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
                invTrxDtl.LotNo = lotNo;

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

            //create itemMasters
            InvItemMaster invItemMaster = new InvItemMaster();
            invItemMaster.InvItemId = item.ItemId;
            invItemMaster.InvItemOriginId = item.OriginId;
            invItemMaster.InvItemBrandId = item.BrandId;
            invItemMaster.LotNo = item.LotNo;
            invItemMaster.BatchNo = item.BatchNo;
            invItemMaster.ItemQty = item.Qty;
            invItemMaster.InvUomId = item.UomId;
            invItemMaster.InvStoreAreaId = item.AreaId;
            //invItemMaster.Remarks = item.Remarks;

            //save changes
            try
            {
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

            //create itemMasters
            InvItemMaster invItemMaster = new InvItemMaster();
            invItemMaster.InvItemId = item.ItemId;
            invItemMaster.InvItemOriginId = item.OriginId;
            invItemMaster.InvItemBrandId = item.BrandId;
            invItemMaster.LotNo = item.LotNo;
            invItemMaster.BatchNo = item.BatchNo;
            invItemMaster.ItemQty = item.Qty;
            invItemMaster.InvUomId = item.UomId;
            invItemMaster.InvStoreAreaId = 1;
            //invItemMaster.Remarks = item.Remarks;

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
        public async Task<ObjectResult> PostReceivingItemEdit(ReceivingTrxItemApiModel item)
        {
            if (item == null)
            {
                return StatusCode(500, "Post Error. Header Details not found.");
            }


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
            public string Uom { get; set; }
            public string Status { get; set; }
        }


    }

}


