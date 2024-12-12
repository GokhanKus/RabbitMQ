# **RabbitMQ**
Bu proje, RabbitMQ kullanarak farklı mesajlaşma modellerinin uygulandığı örnek bir projedir. Projede, çeşitli exchange türleri ve işleyişlerini göstermek için farklı branch'ler ve commit'ler üzerinden geliştirmeler yapılmıştır ve
ilgili başlıklar için, ilgili ekran görüntüleri paylaşılmıştır.


# Branchler ve Açıklamaları

## **1- Fanout-Exchange**

#### Açıklama:
- Mesaj, routing key dikkate alınmaksızın, exchange'e bağlı tüm kuyruklara yayınlanır.
- Birden fazla tüketicinin aynı mesajı alması gerektiğinde kullanılır.
#### Örnek Kullanım:
- Bir haber bülteni sisteminde, bir mesajı tüm abonelere göndermek.

#### Fanout-Exchange Publisher & Subscriber
![image](https://github.com/user-attachments/assets/684a219e-72b2-41ed-b513-c76a169879bf)


## **2- Direct-Exchange**

#### Açıklama:
- Mesajlar, belirli bir routing key ile gönderilir ve bu routing key’e sahip kuyruklara yönlendirilir.
- Doğrudan eşleşme yapılır; sadece ilgili kuyruk mesajı alır.
#### Örnek Kullanım:
- Bir e-ticaret sisteminde, farklı stok güncellemelerini sadece ilgili kategori kuyruğuna göndermek.

#### Direct-Exchange Publisher & Subscriber
![image](https://github.com/user-attachments/assets/ac01a446-5faf-49d6-bc60-8d00c2276ef3)

## **3- Topic-Exchange**

#### Açıklama:
- Routing key'ler ile joker karakterler (*, #) kullanılarak dinamik bir eşleşme yapılır.
- '*' bir kelimeyi temsil eder.
- '#' bir veya birden fazla kelimeyi temsil eder.
- Esnek ve dinamik yönlendirme sağlar.
#### Örnek Kullanım:
- Hava durumu verilerinde, belirli şehir veya ülke bazında mesaj iletimi.

#### Topic-Exchange Publisher & Subscriber 
(bu örnekte başta Error olan mesajlar tüketildi ve istenirse txt dosyasina da istediğimiz türde logları yazdırabiliriz)
![image](https://github.com/user-attachments/assets/c74f4247-5d48-489a-b879-174b8ce4a3b6)
![image](https://github.com/user-attachments/assets/44727e40-b1ef-43bd-bd38-98dffb4bea87)

## **4- Header-Exchange**

#### Açıklama:
- Routing key yerine mesaj başlıklarına (headers) dayanarak yönlendirme yapılır.
- Mesajın başlıklarında belirtilen anahtar-değer çiftleri üzerinden eşleşme sağlanır.
#### Örnek Kullanım:
- Bir uygulamada mesajın tipine veya öncelik seviyesine göre işlem yapılması.

#### Header-Exchange Publisher & Subscriber 
(bu örnekte mesaj olarak string gibi basit tip yerine complex type gönderiyoruz (product nesnesi))
![image](https://github.com/user-attachments/assets/1d5296f2-ff1c-4b96-b791-bfbc9852eece)

## **5- Water Mark Web Örnek Uygulama MVC APP**

#### Açıklama:
- WaterMarkWeb, RabbitMQ kullanarak bir MVC uygulamasında resimlere otomatik olarak filigran (watermark) ekleme işlemini gerçekleştiren bir projedir.
- Projenin temel amacı, resim yükleme ve işleme süreçlerini RabbitMQ ile destekleyerek asenkron hale getirmektir. Bu sayede işlem yoğunluğu azaltılır ve kullanıcı deneyimi iyileştirilir.

#### Örnek Kullanım:
- Fotoğraf Stüdyoları: Otomatik filigran ekleme için.
- E-Ticaret Siteleri: Ürün görsellerine marka logosu ekleme.
- Medya Paylaşım Platformları: Görsel içeriklerin korunması için filigran ekleme.

### Proje Özellikleri
#### 1- Resim Yükleme
- Kullanıcılar, uygulama üzerinden resim dosyalarını yükleyebilir.
- Yüklenen dosyalar, belirlenen bir depolama alanına kaydedilir.

#### 2- Filigran (Watermark) İşlemi
- Yüklenen resimler, bir arka plan servisi (Worker Service) tarafından işlenir.
- Resimlere belirlenen bir metin veya logo filigran olarak eklenir.

#### 3- Asenkron İşleme
- Resim işleme süreci, RabbitMQ kuyrukları kullanılarak asenkron bir şekilde gerçekleştirilir.
- Kullanıcı, resmini yükledikten hemen sonra işlem tamamlanmadan diğer işlemlerine devam edebilir.

#### 4- RabbitMQ Entegrasyonu
- Mesajlar, RabbitMQ aracılığıyla kuyruğa alınır.
- Worker Service, kuyruğa gelen mesajları dinler ve işleme alır.

#### 5- Dinamik Arayüz
- Kullanıcıların yükledikleri dosyaları ve işleme sonuçlarını görebileceği basit ve kullanıcı dostu bir arayüz bulunmaktadır.

### Ekran görüntüleri;
| Resim 1                             | Resim 2                             |
|-------------------------------------|-------------------------------------|
![image](https://github.com/user-attachments/assets/9d02510a-da41-4c11-a9ce-02420cc8acd5) | ![image](https://github.com/user-attachments/assets/1729680e-6431-4985-8f82-4a896738aa35)

| WaterMark'tan önce                  | WaterMark'tan sonra                 |
|-------------------------------------|-------------------------------------|
| ![image](https://github.com/user-attachments/assets/a623c03c-3fc5-4802-a5d6-1c8e36a50e0a) | ![image](https://github.com/user-attachments/assets/ac2ac15a-4783-49b9-bb06-85572daa546e) |


## **6- Excel Web Örnek Uygulama MVC APP**

#### Açıklama:
- ExcelWeb, RabbitMQ ve SignalR kullanarak bir MVC uygulamasında asenkron bir şekilde Excel dosyası oluşturma ve indirme işlemlerini gerçekleştiren bir projedir.
- Bu proje, büyük veri setlerini Excel formatında dışa aktarmak, kullanıcıya anlık bilgilendirmeler yapmak ve işlemleri arka planda yönetmek amacıyla geliştirilmiştir.

### Örnek Kullanım Alanları:
#### E-ticaret Siteleri:
- Kullanım Durumu: Kullanıcılar, büyük miktarda ürün verisini (stok durumu, fiyatlar, vb.) Excel formatında dışa aktarmak isteyebilir. Bu tür veriler genellikle büyük ve karmaşıktır, bu yüzden işlem asenkron hale getirilir.
- Nasıl Kullanılır: RabbitMQ, ürün verilerinin dışa aktarılma talebini alır ve arka planda Excel dosyasını oluşturur. SignalR, işlem başladığında ve tamamlandığında kullanıcıyı bilgilendirir.

#### Fotoğraf ve Medya Yönetim Sistemleri
- Kullanım Durumu: Fotoğraf ve medya içeriklerini yöneten sistemlerde, kullanıcılar yüzlerce veya binlerce medya dosyasını içeren bir rapor veya dosya istediklerinde, bu dosyaların işlenmesi uzun sürebilir.
- Nasıl Kullanılır: SignalR ile işlem durumları kullanıcıya bildirilirken, RabbitMQ arka planda işlemi sıraya alır ve medya verisini uygun formatta dışa aktarır.

#### Eğitim ve Öğrenci Yönetim Sistemleri
- Kullanım Durumu: Eğitim kurumları, öğrenci bilgilerini ve ders performanslarını içeren büyük veri kümelerini dışa aktarmak isteyebilirler.
- Nasıl Kullanılır: RabbitMQ, öğrenci verilerinin dışa aktarılmasını sıraya koyar ve SignalR, öğretmenlere ve yöneticilere her aşamada bildirim gönderir.

## Proje Özellikleri

#### 1- Excel Dosyası Oluşturma
- Kullanıcılar, belirli bir veri setini seçerek Excel dosyası talep edebilir.
- Excel dosyası, arka planda oluşturularak kullanıcının indirmesi için hazır hale getirilir.

#### 2- Asenkron İşleme
- Excel oluşturma işlemi RabbitMQ kuyruğuna gönderilir ve arka planda bir Worker Service tarafından gerçekleştirilir.
- Kullanıcı işlemlerine kesintisiz devam edebilir.

#### 3- SignalR ile Gerçek Zamanlı Bildirimler
- İşlem başladığında ve tamamlandığında, SignalR kullanılarak kullanıcıya gerçek zamanlı bildirim gönderilir.
- Kullanıcılar, işlem durumunu arayüzden anlık olarak takip edebilir.

#### 4- RabbitMQ Entegrasyonu
- Excel oluşturma talepleri, RabbitMQ kuyruğuna mesaj olarak iletilir.
- Worker Service, kuyruğa gelen mesajları dinler ve işlemi tamamlar.

#### 5- Kullanıcı Dostu Arayüz
- Kullanıcıların Excel dosyası taleplerini yönetebileceği ve tamamlanan dosyaları görüntüleyip indirebileceği bir arayüz sunulmaktadır.


## Proje Yapısı

### Frontend
- View Sayfaları: Kullanıcıların Excel dosyası taleplerini oluşturabileceği ve sonuçlarını görüntüleyebileceği bir arayüz sunar.
- SignalR ile Bildirimler: İşlem başlangıcı ve tamamlanması anında, kullanıcıya dinamik bildirimler sunar.
- SweetAlert2 Entegrasyonu: İşlem bilgilerini görsel olarak kullanıcıya iletir.

### Backend
- Controller: Excel oluşturma ve dosya listeleme işlemlerini yönetir.
- RabbitMQ Publisher: Excel oluşturma taleplerini RabbitMQ kuyruğuna iletir.
- Worker Service: Kuyruktaki mesajları alarak Excel dosyası oluşturur ve tamamlandığında SignalR ile kullanıcıyı bilgilendirir.
- SignalR Hub: Gerçek zamanlı bildirimleri yöneten yapı.


## Kullanım Senaryosu
#### 1. Excel Talebi Gönderme:
- Kullanıcı, arayüz üzerinden belirli bir veri setini seçerek Excel dosyası talep eder.

#### 2. Mesaj Kuyruğu:
- Talep edilen işlem, RabbitMQ kuyruğuna mesaj olarak iletilir.

#### 3. Gerçek Zamanlı Bildirim:
- İşlem başladığında SignalR aracılığıyla kullanıcı bilgilendirilir.

#### 4. Dosya Oluşturma:
- Worker Service, kuyruğa gelen mesajı işleyerek Excel dosyasını oluşturur.

#### 5. Tamamlama Bildirimi:
- SignalR ile kullanıcıya işlem tamamlandığı bilgisi gönderilir ve dosya indirmeye hazır hale gelir.

#### 6. Sonuç Görüntüleme ve İndirme:
- Kullanıcı, oluşturulan dosyayı arayüzden görüntüleyebilir ve indirebilir.

| Resim 1                             | Resim 2                             |
|-------------------------------------|-------------------------------------|
![image](https://github.com/user-attachments/assets/10b51769-d3ee-4e9e-b3a3-1ff7a08ef095) | ![image](https://github.com/user-attachments/assets/b72bca64-093c-4639-9ac8-4153fdf91ca3)

| Resim 1                             | Resim 2                             |
|-------------------------------------|-------------------------------------|
![image](https://github.com/user-attachments/assets/a84db80a-acc1-4f84-949e-50c59232cb8b) | ![image](https://github.com/user-attachments/assets/1f1551bb-de87-473c-9192-d75541e877f5)

![image](https://github.com/user-attachments/assets/484ee98a-3a75-446b-b19b-f4235c3e8f23)




