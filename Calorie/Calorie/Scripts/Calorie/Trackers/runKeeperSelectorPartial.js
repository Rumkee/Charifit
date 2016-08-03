


    //function runKeeperPastActivityResponse(response) {

        

    //    response.items.forEach(function (itm) {

    //        itm.duration_mins = itm.duration / 60.0;
    //        itm.total_distanceFormatted = calorie.helpers.toTwoDP(itm.total_distance);
    //        itm.duration_minsFormatted = calorie.helpers.toTwoDP(itm.duration_mins);
                        
    //        var summarySource = $("#RunKeeperActivitySummary-template").html();
    //        var summaryTemplate = Handlebars.compile(summarySource);
                        
    //        if (runKeeperUsedIdents.indexOf(itm.uri) > -1) {
    //            $("#PreviouslyUsed").append(summaryTemplate(itm))
    //        } else {

    //            if (runKeeperAllowedActivities.indexOf(itm.type) == -1 && runKeeperAllowedActivities.length>0) {
    //                $("#UnUsable").append(summaryTemplate(itm))
    //            } else {
    //                calorie.runKeeper.getSpecificActivityData($('#RunKeeperSelector').data('token').toString(), itm.uri, runKeeperSpecificDataResponse, runKeeperSpecificDataResponseError)                    
    //                }

    //      }

    //    })

    //    $('#RunKeeperHeader').children('.spinner').remove()
    //}

    //function runKeeperPastActivityReponseError(err) {
 
    //    $("#Usable").append("Something went wrong getting the data!")
    //    $('#RunKeeperHeader').children('.spinner').remove()

    //}

    //function runKeeperSpecificDataResponse(response) {
             
  
    //    calorie.runKeeper.addPropsToResponse(response)
    //    var source = $("#RunKeeperActivity-template").html();
    //    var template = Handlebars.compile(source);       
              
    //    $("#Usable").append(template(response))

    //    //add gallery
    //    $('#' + response.galleryID).magnificPopup({
    //        delegate: 'a', // child items selector, by clicking on it popup will open
    //        type: 'image',
    //        gallery: {
    //            enabled: true
    //        }
    //        // other options
    //    });

    //    //add map
    //    calorie.helpers.addMap(response.mapId, response.path);


    //}

    //function runKeeperSpecificDataResponseError(err) {
       
    //}




    //function RunKeeperActivityClick(event) {
    //    var blob = $(event.currentTarget).data('jsonblob');
    //    var uniqueID = $(event.currentTarget).data('activityurl');
    //    var thisPanel = $(event.currentTarget.parentNode.parentNode.parentNode.parentNode.parentNode);
    //    thisPanel.find("#addBtnGroup").remove();
        

    //    $('#offSetPanelBody').empty();
    //    $('#offSetPanelBody').append(thisPanel);
    //    $('#RunKeeperSelector').modal('hide');
    //    $('#Description').val(blob.type);
    //    $('#OffsetAmount').val(offSetsCreate.getoffSetAmount(blob.total_distance, blob.duration, blob.total_calories));
    //    $('#BlobType').val('runKeeperActivity');
    //    $('#JSONBlob').val(JSON.stringify(blob));
    //    $('#ThirdPartyIdentifier').val(uniqueID);
        

    //    var Message = offSetsCreate.getActivityDescription(blob.total_distance ,blob.duration ,blob.total_calories)

    //    $('#offSetPanelBodyMessage').append(Message);
    //    $('#CreateOffsetBtn').removeClass("disabled");
        
      

                
    //}