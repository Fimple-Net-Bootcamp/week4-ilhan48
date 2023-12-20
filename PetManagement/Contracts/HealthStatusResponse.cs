namespace PetManagement.Contracts;

public class HealthStatusResponse
{
    public DateTime ExaminationDate { get; set; }
    public bool VaccinationStatus { get; set; }
    public string DiseaseStatus { get; set; }
    public string TreatmentInfo { get; set; }
    public string Notes { get; set; }
    public int PetId { get; set; }
}
