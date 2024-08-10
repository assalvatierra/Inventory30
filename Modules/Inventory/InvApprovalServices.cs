using CoreLib.Interfaces;
using CoreLib.Interfaces.DBAccess;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Inventory.DBAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory
{
    public class InvApprovalServices : IInvApprovalServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly DBMasterService dbMaster;

        public InvApprovalServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            dbMaster = new DBMasterService(_context, _logger);
        }
        public void CreatePoApproval(InvPOApproval invPOApproval)
        {
            try
            {
                dbMaster.InvPOApprovalDb.Create(invPOApproval);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvApprovalServices: Unable to CreatePoApproval :" + ex.Message);
                throw new Exception("InvApprovalServices: Unable to CreatePoApproval :" + ex.Message);

            }
        }

        public void CreateTrxApproval(InvTrxApproval invTrxApproval)
        {
            try
            {
                dbMaster.InvTrxApprovalDb.Create(invTrxApproval);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvApprovalServices: Unable to CreateTrxApproval :" + ex.Message);
                throw new Exception("InvApprovalServices: Unable to CreateTrxApproval :" + ex.Message);

            }
        }

        public void DeletePoApproval(InvPOApproval invPOApproval)
        {
            try
            {
                dbMaster.InvPOApprovalDb.Delete(invPOApproval);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvApprovalServices: Unable to DeletePoApproval :" + ex.Message);
                throw new Exception("InvApprovalServices: Unable to DeletePoApproval :" + ex.Message);

            }
        }

        public void DeleteTrxApproval(InvTrxApproval invTrxApproval)
        {
            try
            {
                dbMaster.InvTrxApprovalDb.Delete(invTrxApproval);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvApprovalServices: Unable to DeleteTrxApproval :" + ex.Message);
                throw new Exception("InvApprovalServices: Unable to DeleteTrxApproval :" + ex.Message);

            }
        }

        public void EditPoApproval(InvPOApproval invPOApproval)
        {
            try
            {
                dbMaster.InvPOApprovalDb.Edit(invPOApproval);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvApprovalServices: Unable to EditPoApproval :" + ex.Message);
                throw new Exception("InvApprovalServices: Unable to EditPoApproval :" + ex.Message);

            }
        }

        public void EditTrxApproval(InvTrxApproval invTrxApproval)
        {
            try
            {
                dbMaster.InvTrxApprovalDb.Edit(invTrxApproval);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvApprovalServices: Unable to EditTrxApproval :" + ex.Message);
                throw new Exception("InvApprovalServices: Unable to EditTrxApproval :" + ex.Message);

            }
        }

        public bool InvPoApprovalExists(int id)
        {
            try
            {
                return dbMaster.InvPOApprovalDb.CheckExists(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvApprovalServices: Unable to InvPoApprovalExists :" + ex.Message);
                throw new Exception("InvApprovalServices: Unable to InvPoApprovalExists :" + ex.Message);

            }
        }

        public bool InvTrxCheckHaveApprovalExist(int TrxId)
        {
            try { 
                return _context.InvTrxApprovals.Any(c => c.InvTrxHdrId == TrxId);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvApprovalServices: Unable to InvTrxCheckHaveApprovalExist :" + ex.Message);
                throw new Exception("InvApprovalServices: Unable to InvTrxCheckHaveApprovalExist :" + ex.Message);

            }
        }

        public bool InvTrxApprovalExists(int id)
        {
            try
            {
                return dbMaster.InvTrxApprovalDb.CheckExists(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvApprovalServices: Unable to InvPoApprovalExists :" + ex.Message);
                throw new Exception("InvApprovalServices: Unable to InvPoApprovalExists :" + ex.Message);

            }
        }

        public InvTrxApproval GetExistingApproval(int TrxId)
        {
            try
            {
                int approvalId = _context.InvTrxApprovals.Where(c => c.InvTrxHdrId == TrxId).Select(c=>c.Id).First();

                var approval = _context.InvTrxApprovals.Find(approvalId);

                return approval;
            }
            catch (Exception ex)
            {
                _logger.LogError("InvApprovalServices: Unable to InvPoApprovalExists :" + ex.Message);
                throw new Exception("InvApprovalServices: Unable to InvPoApprovalExists :" + ex.Message);

            }
        }

        public bool CheckForApprovalStatus(int id)
        {
            try
            {
                var trxHeader = _context.InvTrxHdrs.Where(t => t.Id == id).Include(i=>i.InvTrxType).First();
                var trxApproval = _context.InvTrxApprovals.Where(t=>t.InvTrxHdrId == id).First();

                if (trxApproval!= null)
                {
                    if (trxHeader.InvTrxType.Type == "Receive")
                    {
                        if (!string.IsNullOrEmpty(trxApproval.ApprovedBy) && !string.IsNullOrEmpty(trxApproval.VerifiedBy))
                        {
                            return true;
                        }
                    }

                    if (trxHeader.InvTrxType.Type == "Release")
                    {
                        if (!string.IsNullOrEmpty(trxApproval.ApprovedBy) && !string.IsNullOrEmpty(trxApproval.VerifiedBy) && !string.IsNullOrEmpty(trxApproval.ApprovedAccBy))
                        {
                            return true;
                        }
                    }


                    if (trxHeader.InvTrxType.Type == "Adjust")
                    {
                        if (!string.IsNullOrEmpty(trxApproval.ApprovedBy))
                        {
                            return true;
                        }
                    }
                }

                return false;

            }catch(Exception ex)
            {
                return false;
            }
        }


        public async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }
    }
}
