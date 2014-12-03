$(function() {
	/*
	Menu for mobile resolution
	*/
	$('.to-menu').click(function(e) {
		e.preventDefault();
		$('html, body').css({overflow: 'hidden'});
		$('#mob-nav_outer').animate({left: '0'}, 300);
	});
	$('.close-menu').click(function(e) {
		e.preventDefault();
		$('html, body').css({overflow: 'auto'});
		$('#mob-nav_outer').animate({left: '-100%'}, 300);
	});
});