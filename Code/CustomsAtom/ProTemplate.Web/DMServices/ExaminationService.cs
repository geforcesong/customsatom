
namespace ProTemplate.Web.DMServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using ProTemplate.Web;


    // Implements application logic using the CustomsAtomEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    public partial class CustomsAtomService : LinqToEntitiesDomainService<CustomsAtomEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Examination' query.
        public IList<Examination> GetExaminationsByReceiveDate(int userID, DateTime from, DateTime to)
        {
            var item= this.ObjectContext.GetExaminationByReceiveData(userID, from, to).ToList();
            foreach(var e in item)
                ObjectContext.LoadProperty<Examination>(e, a => a.Customer);
            return item.ToList();
        }

        public IList<Examination> GetExaminationsByNumber(int userID,string numbers)
        {
            var item = this.ObjectContext.GetExaminationByNumber(userID, numbers).ToList();
            foreach (var e in item)
                ObjectContext.LoadProperty<Examination>(e, a => a.Customer);
            return item.ToList();
        }

        public List<Examination> GetExaminationByIDs(List<int> ids)
        {
            var examinations = (from d in this.ObjectContext.Examination
                                where ids.Contains(d.ID)
                                select d).ToList();
            return examinations;
        }

        public List<FinancialExportDeclaration> GetFinancialDeclarationByExaminationIDs(List<int> ids)
        {
            var financialDeclaration = (from e in this.ObjectContext.Examination
                                        from dd in this.ObjectContext.DeclarationDocument
                                        from fd in this.ObjectContext.FinancialExportDeclaration
                                        where e.ExaminationNumber == dd.CertificateNumber && dd.DeclarationId == fd.DeclarationId && fd.FeeTypeCode == "107" && e.ExaminationNumber != ""
                                        select fd).ToList();

            return financialDeclaration;
        }

        public List<DeclarationDocument> GetDeclarationDocumentByExaminationIDs(List<int> ids)
        {
            var declarationDocument = (from e in this.ObjectContext.Examination
                                        from dd in this.ObjectContext.DeclarationDocument
                                        where e.ExaminationNumber == dd.CertificateNumber && e.ExaminationNumber != ""
                                        select dd).ToList();

            return declarationDocument;
        }

        public List<string> GetExistingTransferNumbers()
        {
            var nums = (from d in this.ObjectContext.Examination
                        where d.TransferNumber != null && d.TransferNumber.Trim() != ""
                        select d.TransferNumber).Distinct().ToList();
            return nums;
        }


        public void InsertExamination(Examination examination)
        {
            if ((examination.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(examination, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Examination.AddObject(examination);
            }
        }

        public void UpdateExamination(Examination currentExamination)
        {
            this.ObjectContext.Examination.AttachAsModified(currentExamination, this.ChangeSet.GetOriginal(currentExamination));
        }

        public void DeleteExamination(Examination examination)
        {
            if ((examination.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(examination, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Examination.Attach(examination);
                this.ObjectContext.Examination.DeleteObject(examination);
            }
        }
    }
}


