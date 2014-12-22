if (typeof String.prototype.trim !== 'function')
{
    String.prototype.trim = function ()
    {
        return this.replace(/^\s+|\s+$/g, '');
    };
}


VirtoCommerce = function ()
{
    this.Stores = [];
    this.DynamicContent = {};
};
VirtoCommerce.prototype.constructor = VirtoCommerce.prototype;

VirtoCommerce.prototype = {
    initialize: function ()
    {
    },

    changeStore: function (id)
    {
        $.each(this.Stores, function (index, obj)
        {
            if (obj.Id == id)
            {
                window.location.href = obj.Url;
            }
        });
    },

    changeCurrency: function (id)
    {
        $.redirect(location.href, { currency: id });
    },

    //Register dynamic content
    //banner: place name
    //selector: jquery selector or callback function
    registerDynamicContent: function (banner, selector)
    {
        this.DynamicContent[banner] = selector;
    },

    //This method must be called after all registrations are done. Preferably at the end of base layout
    renderDynamicContent: function ()
    {
        var url = VirtoCommerce.url('/banner/showdynamiccontents');
        var i = 0;
        for (var placeName in this.DynamicContent)
        {
            if (i == 0)
            {
                url = url + '?';
            } else
            {
                url = url + '&';
            }
            url = url + 'placeName=' + placeName;
            i = i + 1;
        }
        if (i > 0)
        {
            $.ajax({
                type: "GET",
                dataType: "html",
                url: url,
                cache: false,
                success: function (data)
                {
                    var htmlData = $('<div/>').html(data);

                    for (var key in VirtoCommerce.DynamicContent)
                    {
                        var selector = VirtoCommerce.DynamicContent[key];
                        var bannerContent = htmlData.find('#' + key);
                        if (bannerContent.length > 0) {
                            var bannerContentHtml = bannerContent.html().trim();
                            if (typeof selector == 'function')
                            {
                                selector(bannerContentHtml);
                            } else
                            {
                                $(selector).html(bannerContentHtml);
                            }
                        }
                    }
                }
            });
        }
    },

    //Input: form object
    //Method serlizes form fields into javascript object
    deserializeForm: function (form)
    {
        var data = {};
        var serialized = form.serializeArray();
        // turn the array of form properties into a regular JavaScript object
        for (var i = 0; i < serialized.length; i++)
        {
            data[serialized[i].name] = serialized[i].value;
        }
        return data;
    },

    //Method tries to create errors object from dictionary [{Key: fieldName, Value: errorMessage}] and shows errors using given validator
    extractErrors: function (jqXhr, validator)
    {

        try
        {
            var data = JSON.parse(jqXhr.responseText); // parse the response into a JavaScript object

            if (data.length != undefined && data.length > 0)
            {

                var errors = {};

                for (var i = 0; i < data.length; i++)
                { // add each error to the errors object
                    errors[data[i].key] = data[i].errors[0];
                }

                validator.showErrors(errors); // show the errors using the validator object
            }
            else if (data.Message != undefined)
            {
                $.showPopupMessage(data.Message);
            }
        }
        catch (ex)
        {
            alert(jqXhr.responseText);
        }
    },

    disableAll: function (selector)
    {
        selector.find('input').attr('disabled', 'disabled');
        selector.find('select').attr('disabled', 'disabled');
        selector.find('textarea').attr('disabled', 'disabled');
    },

    enableAll: function (selector)
    {
        selector.find('input').removeAttr('disabled');
        selector.find('select').removeAttr('disabled');
        selector.find('textarea').removeAttr('disabled');
    },

    url: function (url, force)
    {
        var baseHref = $('base').attr('href');
        if (url.indexOf(baseHref) == 0 && (force == undefined || force == false))
        {
            return url;
        }
        else
        {
            return baseHref + url.substr(1);
        }
    },
    setCookie: function (cname, cvalue, exdays)
    {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toGMTString();
        var path = "path=/";
        document.cookie = cname + "=" + cvalue + "; " + expires + "; " + path;
    },

    getCookie: function (cname)
    {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++)
        {
            var c = ca[i].trim();
            if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
        }
        return "";
    }
};
var VirtoCommerce = new VirtoCommerce();