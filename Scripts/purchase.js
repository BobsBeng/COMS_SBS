
var Categories = []
var incoterm = []
var companyCode = []
var defIncoterm = null
var counter = 1;

function LoadCompanyCodeOnchange(element, url, companyCd, url2) {
    //ajax function for fetch data
    $.ajax({
        type: "GET",
        url: url,
        data: { COMPANY_CD: companyCd, isDDL: "1" },
        success: function (data) {
            companyCode = data;
            //companyCode
            rendercompanyCode(element, companyCode);
        }
    }).done(function () {
        LoadCategory(document.getElementById("productCategory"), url2, companyCd);
    })
}

function LoadCompanyCode(element, url, companyCd) {
    if (companyCode.length == 0) {
        //ajax function for fetch data
        $.ajax({
            type: "GET",
            url: url,
            data: { COMPANY_CD: companyCd },//"companyCD="+companyCd,
            success: function (data) {
                companyCode = data;
                //render catagory
                rendercompanyCode(element);
            }
        })
    }
    else {
        //render company name to the element
        rendercompanyCode(element);
    }
}

function rendercompanyCode(element, companyCd) {
    $.each(companyCd, function (i, val) {
        $('#country').val(val.CNTRY_NM);
        //$('#picName').val(val.CST_CNTCT_PSN_NM);//picName
        $('#currency').val(val.BCUR_CD);//currency
        $('#deliveryDest').val(val.SCST_CD);//deliveryDest
        defIncoterm = val.INCTRMS_CD;
        if (defIncoterm != null) {
            selectElement('incoterm', defIncoterm);
        }

    });

}

function LoadIncoterm(element, url, def) {
    if (incoterm.length == 0) {
        //ajax function for fetch data
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                incoterm = data;
                //render incoterm
                renderIncoterm(element, def);
            }
        })
    }
    else {
        //render incoterm to the element
        renderIncoterm(element, def);
    }
}

function LoadCategory(element, url, CustCD) {
    removeOptions(document.getElementById('productCategory'));
    //ajax function for fetch data
    $.ajax({
        type: "GET",
        data: { CustCD: CustCD },
        url: url,
        success: function (data) {
            Categories = data;
            renderCategory(element);
        }
    });
}

function renderCategory(element) {
    var $ele = $(element);
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Categories, function (i, val) {
        $ele.append($('<option/>').val(val.CST_ITM_CD).text(val.CST_ITM_CD));
    })
}


function renderIncoterm(element, def) {

    var temp = "0";
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(incoterm, function (i, val) {
        $ele.append($('<option/>').val(val.INCTRMS_CD).text(val.INCTRMS_CD + " (" + val.INCTRMS_NM + ")"));
    });

    if (def != null) {
        temp = def;
    } else if (defIncoterm != null) {
        temp = defIncoterm;
    }
    selectElement('incoterm', temp);
}

function LoadProduct(itemCD, url, url2) {
    //Clean error message
    cleanErrorSpan()
    
    $.ajax({
        type: "GET",
        url: url,
        data: { 'ITM_CD': $(itemCD).val() },
        success: function (data) {
            renderProduct($(itemCD).parents('.mycontainer').find('select.product'), data);
        },
        error: function (error) {
            console.log(error);
        }
    }).done(function () {

        $.ajax({
            type: "GET",
            url: url2,
            data: { 'ITM_CD': $(itemCD).val() },
            success: function (data) {
                removeOptions(document.getElementById('UnitCd'));
                renderUnit($(itemCD).parents('.mycontainer').find('select.UnitCd'), data);
            },
            error: function (error) {
                console.log(error);
            }
        });

    });
}

function renderProduct(element, data) {
    //render product
    
    var $ele = $(element);
    $ele.empty();
    $.each(data, function (i, val) {
        $('#itmName').val(val.CST_ITM_NM);
    })
}

function renderUnit(element, data) {
    var $ele = $(element);
    var temp = "";
    $.each(data, function (i, option) {
        if (option.value !== null || option.value !== undefined || option.value !== '') {
            $('#UnitCd').append($('<option/>').attr("value", option.UNIT_CD).text(option.UNIT_CD));
            temp = option.BASE_UNIT_CD;
        }
    });
    selectElement('UnitCd', temp);
}

function selectElement(id, valueToSelect) {
    let element = document.getElementById(id);
    element.value = valueToSelect;
}

function removeOptions(selectElement) {
    var i, L = selectElement.options.length - 1;
    for (i = L; i >= 0; i--) {
        selectElement.remove(i);
    }
}

function formatAmount(value) {
    // Add commas for digit grouping
    var parts = value.toString().split('.');
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');

    // Concatenate parts
    return parts.join('.');
}

function calculateSum() {
    var sum = 0;

    rate = parseFloat($('#unitPrice').val().replace(/,/g, '')) || 0;
    qty = parseFloat($('#quantity').val().replace(/,/g, '')) || 0;
    total = parseFloat((qty * rate)).toFixed(4);
    var formatValue = total;

    $('#rate').val(new Intl.NumberFormat().format(formatValue));
}

function getDate() {
    var d = new Date();
    var strDate = d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate();
    return strDate
}

function numberWithCommas(x) {
    // Add commas for digit grouping
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

$('#companyCode').click(function () {
    var e = document.getElementById("companyCode");
    var value = e.value;
    var text = e.options[e.selectedIndex].text;
    $('#count').val(value);
});

function setInputFilter(textbox, inputFilter, errMsg) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop", "focusout"].forEach(function (event) {
        textbox.addEventListener(event, function (e) {
            if (inputFilter(this.value)) {
                // Accepted value
                if (["keydown", "mousedown", "focusout"].indexOf(e.type) >= 0) {
                    this.classList.remove("input-error");
                    this.setCustomValidity("");
                }
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                // Rejected value - restore the previous one
                this.classList.add("input-error");
                this.setCustomValidity(errMsg);
                this.reportValidity();
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                // Rejected value - nothing to restore
                this.value = "";
            }
        });
    });
}

$(document).on('keyup', '.price', function () {
    var x = $(this).val() || 0;
    var y = 0;
    calculateSum();

    if (x.length > 0) {
        var numVal = x.split('.');
        if (numVal.length > 1) {
            x = numVal[0].replace(/,/g, '');
            y = numVal[1].replace(/,/g, '').substring(0, 6);
            $(this).val(x.toString().replace(/,/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",").concat(".".concat(y)));
        } else {
            $(this).val(x.toString().replace(/,/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        }
    }
});

$(document).on('keyup', '.qty', function () {
    var x = $(this).val() || 0;
    var y = 0;
    calculateSum();

    if (x.length > 0) {
        var numVal = x.split('.');
        if (numVal.length > 1) {
            x = numVal[0].replace(/,/g, '');
            y = numVal[1].replace(/,/g, '').substring(0, 3);
            $(this).val(x.toString().replace(/,/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",").concat(".".concat(y)));
        } else {
            $(this).val(x.toString().replace(/,/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        }
    }
});

function cleanErrorSpan() {

    [].forEach.call(document.querySelectorAll('span.error'), function (el) {
        el.style.visibility = 'hidden';
    });
}

function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}

function parseDate(dateString) {
    var parts = dateString.split("/");    
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

function addLine(url) {
    var min = 0;
    var max = 0;
    var lt = 0;
    var unitCD = $('#UnitCd').val();
    var jsondata;
    var itm_Cd = document.getElementById("productCategory").value;
    $.ajax({
        type: "GET",
        url: url,
        async: false,
        dataType: 'json',
        data: { itm_Cd: itm_Cd, unit_Cd: unitCD },
        success: function (Data) {
            jsondata = Data;
            $.each(jsondata, function (i, obj) {
                min = obj.min;
                max = obj.max;
                lt = obj.lt;
                unit = obj.unit_Cd;
                stdPckg = obj.standardPacking;
            });
        }

    });
    debugger
    //validation and add order items
    var isAllValid = true;
    if ($('#productCategory').val() == "0") {
        isAllValid = false;
        $('#productCategory').siblings('span.error').css('visibility', 'visible');
    }
    else {
        $('#productCategory').siblings('span.error').css('visibility', 'hidden');
    }

    if (parseFloat($('#quantity').val().replace(/,/g, "") || 0) == 0) {
        isAllValid = false;
        $('#lblquantity').html("Valid quantity is required")
        $('#quantity').siblings('span.error').css('visibility', 'visible');
    } else if (parseFloat($('#quantity').val().replace(/,/g, "") || 0) < min & min > 0) {
        isAllValid = false;
        $('#lblquantity').html("Min. quantity: ".concat(min.toString().concat(" " + unit)));
        $('#quantity').siblings('span.error').css('visibility', 'visible');
    } else if (parseFloat($('#quantity').val().replace(/,/g, "") || 0) > max & max > 0) {
        isAllValid = false;
        $('#lblquantity').html("Max. quantity: ".concat(max.toString().concat(" " + unit)));
        $('#quantity').siblings('span.error').css('visibility', 'visible');
    } else if ($('#quantity').val().replace(/,/g, "") % stdPckg !== 0) {
        // Raise an error here
        isAllValid = false;
        $('#lblquantity').html("Quantity must be a multiple of " + stdPckg);
        $('#quantity').siblings('span.error').css('visibility', 'visible');
    }
    else {
        $('#quantity').siblings('span.error').css('visibility', 'hidden');
    }

    if (parseFloat($('#unitPrice').val().replace(/,/g, "") || 0) == 0) {
        isAllValid = false;
        $('#unitPrice').siblings('span.error').css('visibility', 'visible');
    }
    else {
        $('#unitPrice').siblings('span.error').css('visibility', 'hidden');
    }

    if ($('#UnitCd').val() == "0" || ($('#UnitCd').val() == '')) {
        isAllValid = false;
        $('#UnitCd').siblings('span.error').css('visibility', 'visible');
    }
    else {
        $('#UnitCd').siblings('span.error').css('visibility', 'hidden');
    }

    if (!($('#rate').val().trim() != '')) {
        isAllValid = false;
        $('#rate').siblings('span.error').css('visibility', 'visible');
    }
    else {
        $('#rate').siblings('span.error').css('visibility', 'hidden');
    }
    
    if (!($('#deliveryDate').val().trim() != '')) {
        isAllValid = false;
        $('#lbldeliveryDate').html("Delivery date must be defined");
        $('#deliveryDate').siblings('span.error').css('visibility', 'visible');
    }
    else if (addDays(parseDate($('#orderDate').val()), lt) > parseDate($('#deliveryDate').val())) {
        isAllValid = false;
        $('#lbldeliveryDate').html("Delivery lead time: ".concat(lt.toString().concat(" days")));
        $('#deliveryDate').siblings('span.error').css('visibility', 'visible');
    }
    else {
        $('#deliveryDate').siblings('span.error').css('visibility', 'hidden');
    }

    if (isAllValid) {
        var t = document.getElementById("productCategory");
        var shipperDdlValue = document.getElementById("ddlShipper").value;
        $('#ddlshipper').prop('disabled', true);
        if (shipperDdlValue === "ETA") {
            // If shipperDdlValue is not "ETA", set the value for $('#deliveryDate')
            var deliveryDateValue = $('#deliveryDate').val();
        } else {
            // If shipperDdlValue is "ETA", set a different value or leave it empty
            var deliveryDateValue = ""; 
        }
        if (shipperDdlValue === "ETD") {
            // If shipperDdlValue is not "ETA", set the value for $('#deliveryDate')
            var deliveryDateValue2 = $('#deliveryDate').val();
        } else {
            // If shipperDdlValue is "ETA", set a different value or leave it empty
            var deliveryDateValue2 = ""; 
        }        
        var dummyApprvd = "";
        var dummyCancel = "";
        var dummyRemarks = "";
        var quantityValue = parseFloat($('#quantity').val().replace(/,/g, ''));
        var unitPriceValue = parseFloat($('#unitPrice').val().replace(/,/g, ''));
        var rateValue = parseFloat($('#rate').val().replace(/,/g, ''));
        var t = $('#inputDetailsItems').DataTable();
        var allData = t.data();
        var totalRows = allData.length;
        if (totalRows !== "") {
            counter = parseInt(totalRows, 10) + 1;
        } else {
            counter = 1;
        }
        console.log(counter);
        t.row.add([
                       counter++,
                       $('#productCategory').val(),
                       $('#itmName').val(),
                       quantityValue,
                       $('#UnitCd').val(),
                      unitPriceValue,
                       rateValue,
                      deliveryDateValue,
                      deliveryDateValue2,
                      dummyApprvd,
                      dummyCancel,
                      dummyRemarks,
                      "<button type='button' id='deleteRow' class='btn btn-danger btn-sm' title='Remove'><i class='fas fa-window-close'></i></button>"
        ]).draw();

        //clear select data
        $('#productCategory,#product,#UnitCd').val('0');
        $('#UnitCd').empty();
        $('#quantity,#rate,#itmName,#unitPrice,#deliveryDate').val('');
        $('#orderItemError').empty();
    }

}


function saveData(url, saveStatus, uTate, indexUrl) {
   
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    });

    var detailOrder = new Array();
    var getAll = $('#inputDetailsItems').DataTable().column(1).data().toArray();

    if (getAll.length == 0) {
        $('#myModal').modal('hide');
        Toast.fire({
            icon: 'warning',
            title: 'No data row!'
        });
        return;
    }

    for (let i = 0; i < getAll.length; i++) {
        var orderList = {
            ITM_CD: $('#inputDetailsItems').DataTable().cell(i, 1).data().trim(),
            ITM_NM: $('#inputDetailsItems').DataTable().cell(i, 2).data().trim(),
            INPT_QTY: $('#inputDetailsItems').DataTable().cell(i, 3).data(),
            INPT_UNIT_CD: $('#inputDetailsItems').DataTable().cell(i, 4).data().trim(),
            UPRI: $('#inputDetailsItems').DataTable().cell(i, 5).data(),
            RQST_ARVD_DT: parseDate($('#inputDetailsItems').DataTable().cell(i, 7).data().trim()),
            RQST_DLV_DT: parseDate($('#inputDetailsItems').DataTable().cell(i, 8).data().trim()),
          
        }
        detailOrder.push(orderList);
    }

    var sumOrder = {
        T_SLO_NO: $('#orderNo').val(),
        CST_PO_NO: $('#orderNo2').val(),
        CST_CD: $('#custCode').val(),
        CST_CNTCT_PSN_NM: $('#picName').val(),
        SCST_CD: $('#deliveryDest').val().trim(),
        T_SLO_DT: parseDate($('#orderDate').val()),
        SLIP_RMRKS: $('#description').val().trim(),
        CUR_CD: $('#currency').val().trim(),
        INCTRMS_CD: $('#incoterm').val().trim(),
        PO_STS: saveStatus,
        INTR_RMRKS: $('#ddlShipper2').val(),
        OrderD: detailOrder
    }
   
    $.ajax({
        type: 'POST',
        url: url,
        data: '{ order:' + JSON.stringify(sumOrder) + ', actionID:' + saveStatus + ', state:' + uTate + '}',
        contentType: 'application/json',
        cache: false,
        success: function (data) {
            $('#myModal').modal('hide');  
            Toast.fire({
                icon: data.MID,
                title: data.MSG,
            }).then(function () {
                if (data.MID != "warning" && data.VAL == null) {
                    window.location = indexUrl;
                }
                if (data.MID == "success" && data.VAL != null) {
                    window.location = indexUrl +'?id='+ data.VAL;
                }
            });
        },
        error: function (error) {
            $('#myModal').modal('hide');
            Toast.fire({
                icon: 'error',
                title: error
            });
        }
    });
}

function deleteData(url, indexUrl) {

    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    });

    PO = $('#orderNo').val().trim();
    console.log(url + "/" + PO);
    $.ajax({
        type: 'POST',
        url: url,
        data: '{ poNumber:' + JSON.stringify(PO) +'}',
        contentType: 'application/json',
        cache: false,
        success: function (data) {
            $('#myModal').modal('hide');
            Toast.fire({
                icon: data.MID,
                title: data.MSG,
            }).then(function () {
                if (data.MID != "warning" && data.VAL == null) {
                    window.location = indexUrl;
                }
                if (data.MID == "success" && data.VAL != null) {
                    window.location = indexUrl + data.VAL;
                }
            });
        },
        error: function (error) {
            $('#myModal').modal('hide');
            Toast.fire({
                icon: 'error',
                title: error
            });
        }
    });
}
