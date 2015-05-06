$.url = function (url) {
	return $("base").attr("href") + url.substr(1);
}

$(document).ready(function () {
	$('[data-id="buySpan_top"]').off('click').on('click', function (e) {
		if (!$('[data-id="buyButton_top"]').is('.__disabled')) {
			$('[data-id="buyButton_top"]').addClass('__disabled');
		}
		else {
			$('[data-id="buyButton_top"]').removeClass('__disabled');
		}
	});

	$('[data-id="buySpan_bottom"]').off('click').on('click', function (e) {
		if (!$('[data-id="buyButton_bottom"]').is('.__disabled')) {
			$('[data-id="buyButton_bottom"]').addClass('__disabled');
		}
		else {
			$('[data-id="buyButton_bottom"]').removeClass('__disabled');
		}
	});

	$('[data-id="buyButton_top"]').off('click').on('click', function (e) {
		if ($(e.target).is('.__disabled')) {
			e.preventDefault();
		}
	});

	$('[data-id="buyButton_bottom"]').off('click').on('click', function (e) {
		if ($(e.target).is('.__disabled')) {
			e.preventDefault();
		}
	});

	$('#read_all_reviews').on('click', function (e) {
		$('html, body').animate({
			scrollTop: $("#reviews").offset().top - 99
		}, 500);
		MP.openTab($('li_reviews'));
	});

	$('[data-id="reviewFormButton"]').on('click', function () {
		$('#reviewForm').show();
	});
	$('[data-id="becomeDeveloperFormButton"]').on('click', function () {
		$('#becomeDeveloperForm').show();
	});
	$('[data-id="supportVendorFormButton"]').on('click', function () {
		$('#supportVendorForm').show();
	});
	$('[data-id="contactVendorFormButton"]').on('click', function () {
		$('#contactVendorForm').show();
	});

	$('.popup-close, [data-id="closePopup"]').on('click', function () {
		$('.overlay').hide();
	});

	$('[data-id="submitPopup"]').on('click', function (e) {
		var formId = $(e.target).attr('data-formId');

		//if ($('[data-id="' + formId + '"]').valid()) {
		//	alert('desc');
		//}

		$('[data-id="' + formId + '"]').submit();
	});

	$('#searchInput').on('keyup', function (e) {
		if (Search.request !== undefined) {
			Search.request.abort();
		}

		Search.request = $.ajax({
			method: 'GET',
			url: $.url('/search/find?q=' + $(e.target).val()),
			success: function (data) {
				$('.autocomplete').remove();

				if (data !== undefined) {
					if (data.length > 0) {
						var html = '<div class="autocomplete"><ul class="list">';

						for (var i = 0; i < data.length; i++) {
							html = html + '<li class="list-item">'
							+ '<a class="list-link" href="' + data[i].url + '">' + data[i].value + '</a>'
							+ '</li>';
						}

						html = html + '</ul></div>';

						$('form.header-search').append(html);
					}
				}
			}
		})
	});

	Module.initilizeRatingForm();
})

var Module = {
	initilizeRatingForm: function () {
		$('.overlay #rating li').on('mouseenter', function (e) {

			var pref = $(e.target).attr('data-id');
			$('.rating-eval').hide();
			$('#' + pref).show();
			$('#rating').removeClass('__bad').removeClass('__not-bad').removeClass('__normal').removeClass('__good').removeClass('__nice');
			$('#rating').addClass(pref);

		}).on('mouseleave', function (e) {

			$('#rating').removeClass('__bad').removeClass('__not-bad').removeClass('__normal').removeClass('__good').removeClass('__nice');
			if ($('[data-choosen="true"]').length > 0) {
				var pref = $('[data-choosen="true"]').attr('data-id');
				$('#rating').addClass(pref);
				$('.rating-eval').hide();
				$('#' + pref).show();
			}

		}).on('click', function (e) {

			var pref = $(e.target).attr('data-id');
			var rat = $(e.target).attr('data-value');
			$('#rating li').attr('data-choosen', 'false');
			$(e.target).attr('data-choosen', 'true');
			$('.rating-eval').hide();
			$('#' + pref).show();
			$('#rating').removeClass('__bad').removeClass('__not-bad').removeClass('__normal').removeClass('__good').removeClass('__nice');
			$('#rating').addClass(pref);
			$('[name="rating"]').val(rat);

		});
	}
}

var Search = {
	request: undefined
}
