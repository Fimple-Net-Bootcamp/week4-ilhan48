namespace PetManagement.Entities;

public class HealthStatus : BaseEntity<int>
{
    public DateTime ExaminationDate { get; set; }
    public bool VaccinationStatus { get; set; }
    public string DiseaseStatus { get; set; }
    public string TreatmentInfo { get; set; }
    public string Notes { get; set; }

    public int PetId { get; set; }
    public virtual Pet Pet { get; set; }

    public HealthStatus()
    {
        ExaminationDate = DateTime.UtcNow;
        VaccinationStatus = false;
        DiseaseStatus = string.Empty;
        TreatmentInfo = string.Empty;
        Notes = string.Empty;
    }

    public HealthStatus(
        DateTime examinationDate,
        bool vaccinationStatus,
        string diseaseStatus,
        string treatmentInfo,
        string notes
    )
    {
        ExaminationDate = examinationDate;
        VaccinationStatus = vaccinationStatus;
        DiseaseStatus = diseaseStatus;
        TreatmentInfo = treatmentInfo;
        Notes = notes;
    }
    public HealthStatus(
        int id,
        DateTime examinationDate,
        bool vaccinationStatus,
        string diseaseStatus,
        string treatmentInfo,
        string notes
    )
        : base(id)
    {
        ExaminationDate = examinationDate;
        VaccinationStatus = vaccinationStatus;
        DiseaseStatus = diseaseStatus;
        TreatmentInfo = treatmentInfo;
        Notes = notes;
    }
}