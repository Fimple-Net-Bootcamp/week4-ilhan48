# [Evcil Hayvan Yönetimi API]((https://github.com/Fimple-Net-Bootcamp/week4-ilhan48))

## Proje Açıklaması

Bu proje, evcil hayvanların etkili bir şekilde yönetilebilmesi için geliştirilmiştir. Vertical Slice Architecture ve CQRS pattern kullanılarak tasarlanan bu API, .NET7, Entity Framework, Mapster, FluentValidation, MediatR gibi güçlü teknolojileri içerir.

---

# Projeyi Kurma ve Çalıştırma Kılavuzu

## Kurulum

Bu bölüm, projeyi yerel geliştirme ortamınızda başarıyla kurmak ve çalıştırmak için gereken adımları detaylandırmaktadır.

### Adım 1: Repoyu Klonlama
- Projeyi yerel bilgisayarınıza klonlamak için, terminalinize şu komutu girin:
  ```
  git clone https://github.com/Fimple-Net-Bootcamp/week4-ilhan48.git
  ```

### Adım 2: Geliştirme Ortamını Hazırlama
- Visual Studio veya tercih ettiğiniz IDE'yi açın.
- İndirdiğiniz projeyi IDE üzerinden açın.

### Adım 3: Bağımlılıkları Yükleme
- Projede kullanılan kütüphaneler ve bağımlılıklar için, IDE'nizdeki bağımlılık yönetimi aracını kullanarak gerekli paketleri yükleyin. (örneğin Nuget)

### Adım 4: Veritabanı Ayarlarını Yapma
- Projede Entity Framework kullanıyorsanız, veritabanını oluşturmak ve başlangıç verilerini yüklemek için migration komutlarını çalıştırın. Örnek komut:
  ```
  dotnet ef database update
  ```

## Kullanım

### Adım 1: API'yi Başlatma
- Geliştirme ortamınızda, API servisini başlatmak için gerekli komutları çalıştırın.

### Adım 2: API Dokümantasyonunu İnceleme
- API endpoint'lerini ve kullanımlarını anlamak için Swagger ya da benzeri bir API belgelendirme aracını kullanın.

### Adım 3: API İle Etkileşime Geçme
- API'nizle etkileşimde bulunmak için, belirtilen endpoint'lere uygun HTTP istekleri gönderin.

---
