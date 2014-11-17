itemDetail = {
	initializeImages: function() {
		var images = $('.scroll ul.clearfix li');
		var newImages = [];
		if (images.length == 2) {
			var first = $(images[0]).clone();
			var secondwithoutselected = $(images[1]).clone();
			$(secondwithoutselected).removeClass('selected');
			var first2 = $(images[0]).clone();
			var secondwithoutselected2 = $(images[1]).clone();
			$(secondwithoutselected2).removeClass('selected');
			newImages.push(images[0]);
			newImages.push(images[1]);
			newImages.push(first);
			newImages.push(secondwithoutselected);
			newImages.push(first2);
			newImages.push(secondwithoutselected2);
			$('.scroll ul.clearfix').html(newImages);
			itemDetail.images();
		} else if (images.length == 3) {
			var first = $(images[0]).clone();
			var secondwithoutselected = $(images[1]).clone();
			$(secondwithoutselected).removeClass('selected');
			var third = $(images[2]).clone();
			newImages.push(images[0]);
			newImages.push(images[1]);
			newImages.push(images[2]);
			newImages.push(first);
			newImages.push(secondwithoutselected);
			newImages.push(third);
			$('.scroll ul.clearfix').html(newImages);
			itemDetail.images();
		} else if (images.length > 3) {
			itemDetail.images();
		}
	},
	images: function() {
		Animate();
		$(window).resize(function() {
			Animate();
		});

		function Animate() {
			var selected = $('.list-items .scroll .clearfix li.selected');
			var nextitem = selected.next();
			var previtem = selected.prev();
			var prevprevitem = previtem.prev();
			if (nextitem.length > 0) {
				nextitem.css("margin-left", "7px");
			}
			if (previtem.length > 0) {
				previtem.css("margin-left", "0px");
			}
			if (prevprevitem.length > 0) {
				var height = prevprevitem.height();
				var work = true;
				$.each($('.list-items .scroll .clearfix li'), function(index, el) {
					if (!$(el).hasClass('selected') && work) {
						$(el).css("margin-left", '-' + height + 'px');
					} else {
						work = false;
					}
				});
				prevprevitem.css("margin-left", '-' + height + 'px');
			}
		}
		$('.list-items .next').off('click').on('click', function() {
			var block = $('.list-items .scroll .clearfix');
			var selected = $('.list-items .scroll .clearfix li.selected');
			var nextitem = selected.next();
			if (nextitem.length < 1) {
				nextitem = $('.list-items .scroll .clearfix li:first').next();
				nextitem.appendTo(block);
				nextitem.css('margin-left', '7px');
			}
			var nextnextitem = nextitem.next();
			if (nextnextitem.length < 1) {
				nextnextitem = $('.list-items .scroll .clearfix li:first');
				nextnextitem.appendTo(block);
				nextnextitem.css('margin-left', '7px');
			}
			var previtem = selected.prev();
			var height = previtem.height() + 7;
			$(previtem).animate({
				"margin-left": '-=' + height + 'px'
			}, "medium");
			$(selected).animate({
				"margin-left": '0px'
			}, "medium");
			$(nextitem).animate({
				"margin-left": '7px'
			}, "medium");
			$(nextnextitem).animate({
				"margin-left": '7px'
			}, "medium");
			selected.attr("onclick", "$('.list-items .prev').click();");
			nextnextitem.attr("onclick", "$('.list-items .next').click();");
			nextitem.attr("onclick", "");
			var bigImageId = nextitem.attr('data-bigimage-id');
			var image = $('[data-image-id="' + bigImageId + '"]');
			$('[data-image-id]').css('display', 'none');
			image.css('display', 'block');
			selected.removeClass('selected');
			nextitem.addClass('selected');
		});
		$('.list-items .prev').off('click').on('click', function() {
			var block = $('.list-items .scroll .clearfix');
			var selected = $('.list-items .scroll .clearfix li.selected');
			var previtem = selected.prev();
			if (previtem.length < 1) {
				previtem = $('.list-items .scroll .clearfix li:last').prev();
				previtem.prependTo(block);
				previtem.css('margin-left', '7px');
			}
			var prevprevitem = previtem.prev();
			if (prevprevitem.length < 1) {
				prevprevitem = $('.list-items .scroll .clearfix li:last');
				prevprevitem.prependTo(block);
				var height = previtem.height();
				prevprevitem.css('margin-left', '-' + height + 'px');
			}
			var height = previtem.height();
			prevprevitem.animate({
				"margin-left": '+=' + height + 'px'
			}, "medium");
			previtem.animate({
				"margin-left": '7px'
			}, "medium");
			selected.animate({
				"margin-left": '7px'
			}, "medium");
			selected.attr("onclick", "$('.list-items .next').click();");
			prevprevitem.attr("onclick", "$('.list-items .prev').click();");
			previtem.attr("onclick", "");
			var bigImageId = previtem.attr('data-bigimage-id');
			var image = $('[data-image-id="' + bigImageId + '"]');
			$('[data-image-id]').css('display', 'none');
			image.css('display', 'block');
			selected.removeClass('selected');
			previtem.addClass('selected');
		});
	}
}

$(function() {
	itemDetail.initializeImages();
});