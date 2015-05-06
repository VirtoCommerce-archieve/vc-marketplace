var MP = {

    loadTemplate: function(tpl, tplName) {
        //tpl.load(tplName);
        $.post('templates/' + tplName, {}, function (response) {
            tpl.html(response)
        }, 'html');
    },

    openTab: function(tab) {
        tab.addClass('__selected').siblings().removeClass('__selected');
        $('.tab-content').hide().eq(tab.index()).show();
    }

};

$(function () {
    $('.header-mobile_nav').on('click', function () {
        $('body').toggleClass('__opened');
    });

    $('.tabs .tab-content:not(:first)').hide();

    $('.tabs .menu-item').on('click', function () {
        MP.openTab($(this));
    });
});