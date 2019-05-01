$(".deneme").on('click', function () {
    var id = $(this).attr('data-id');
    $.ajax({
        url: '/Kullanici/FavoriEkleSil/' + id,
        type: 'POST',
        success: function (rs) {
            if (rs === true) {
                $('#' + id).remove();
            }
            else if (rs === "cik") {
                alert("Favoriden Çıkıldı.");
            }
            else if (rs === "Gir") {
                alert("Öncelikle Giriş Yapmalısınız.");
            }
            else if (rs === "ekle") {
                alert("Favori Ekleme Başarılı.");
            }
            else {
                alert('Silme işlemi gerçekleşirken bir hata oluştu.');
            }
        },
        error: function (rs) {
            alert('Bir hata oluştu');
        }
    });
})