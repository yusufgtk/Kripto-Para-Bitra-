# Bitra - Blockchain Tabanlı Kripto Para

![Bitra Blockchain Sistemi](./images/giris.png)
![Bitra Blockchain Sistemi](./images/kayit.png)
![Bitra Blockchain Sistemi](./images/anasayfa.png)

Bitra, merkezi bir yapıya sahip, **ASP.NET Core API** ile geliştirilmiş ve **blockchain** tabanlı bir kripto para sistemidir. Sistem, **SignalR** kullanarak ağa katılım sağlar ve **React Native** ile geliştirilmiş bir mobil cüzdan uygulamasına sahiptir.

## Özellikler

- **Merkezi Blockchain Yapısı:** Tüm bloklar sunucu tarafından kazılır.
- **Kazma (Mining) Sistemi:** Her blok kazmada 100 **Bitra** token üretilir, bu miktar belirli blok sayısında yarıya iner.
- **Genesis Block:** Sistemin ilk bloğu.
- **Toplam Arz:** 50 milyon Bitra ile sınırlıdır.
- **Arz-Talep Dengesine Göre Fiyat Belirleme:** Fiyat, kazma zorluğu ve arz-talep oranına göre değişir.
- **SignalR ile Ağa Katılma:** Kullanıcılar SignalR protokolü ile ağa bağlanabilir.
- **React Native Mobil Cüzdan:** Kullanıcılar, mobil uygulama üzerinden Bitra tokenlerini gönderebilir ve alabilir.

---

## Mimari

**Backend:**

- **ASP.NET Core API:** Blockchain'in merkezi sunucu tarafı.
- **Entity Framework Core:** Veritabanı yönetimi.
- **SignalR:** Gerçek zamanlı bağlantılar.

**Frontend:**

- **React Native:** Mobil cüzdan uygulaması.
- **Redux:** Durum yönetimi.

---

## Bitra Madencilik (Mining) Sistemi

- **Blok Kazma:** Sistem otomatik olarak blok kazımı yapar ve ödülü kendi cüzdanına aktarır.
- **Ödül Mekanizması:** İlk etapta her blok başına **100 Bitra** verilir.
- **Yarılama Mekanizması:** Belirli blok aralıklarında (Bitcoin gibi) ödül yarıya düşer.
- **Toplam Arz:** Maksimum **50 milyon** Bitra üretilebilir.

---

## Bitra Transfer ve Cüzdan Kullanımı

Mobil cüzdan uygulaması ile Bitra tokenlerinizi gönderebilir, alabilir ve bakiyenizi görebilirsiniz.

---

## Katkıda Bulunma

Projeye katkıda bulunmak isteyenler **pull request** oluşturabilir veya issue açarak geri bildirimde bulunabilir.

---

## Lisans

MIT Lisansı altında sunulmaktadır.
