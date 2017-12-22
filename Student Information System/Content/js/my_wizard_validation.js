
    $(function () {
        $('#horizontal-wizard').smartWizard({
            enableFinishButton: false,
            onLeaveStep: leaveAStepCallback,
            onFinish: onFinishCallback,
            onShowStep: onShow
        });
        function onShow(obj, context) {
            var step_num = obj.attr('rel');
            if (step_num == "10")
                showResult();
            return true;//validateSteps(step_num); // return false to stay on step and true to continue navigation
        }
        function leaveAStepCallback(obj, context) {
            var step_num = obj.attr('rel');
            return validateSteps(step_num); // return false to stay on step and true to continue navigation
        }
        function onFinishCallback() {
            if (validateAllSteps()) {
                $('#horizontal-wizard').smartWizard('showMessage', 'Finish Clicked');
                $('form').submit();
            }
        }


        //function stepChange(event, currentIndex, newIndex) {
        //    if (newIndex === 3){// && Number($("#age-2").val()) < 18) {
        //        return false;
        //    }
        //}

        function validateAllSteps() {
            var isStepValid = true;
            // all step validation logic     
            return isStepValid;
        }
        function showResult() {

            var fields_name = ["va pg od", "va sc od", "va cc od", "va pg os", "va sc os", "va cc os",
                        "refrection pg od", "refrection sc od", "refrection cc od", "refrection pg os", "refrection sc os", "refrection cc os",
                        // 1
                     //   "Date", "Admission", "Name", "Family", "Father's Name", "Age", "Gender", "melli", "Address", "Tel",
                        // 2
                     //   "cc", "PHMx", "other", "drug_hx", "family_hx", "Other Ocular Disease",
                        // 3
                        "Previous Surgery od", "Previous Surgery os", "Conjunctiva OD", "Conjunctiva OS", "Anterior Chamber OD", "Anterior Chamber OS"];
            var fields_id = ["va_pg_od&&va_sc_od", "va_sc_od", "va_cc_od", "va_pg_os", "va_sc_os", "va_cc_os",
                        "refrection_pg_od", "refrection_sc_od", "refrection_cc_od", "refrection_pg_os", "refrection_sc_os", "refrection_cc_os",
                        // 1
                     //   "exam_date", "sick.s_admission", "sick_s_name", "sick_s_family", "sick_s_father", "sick_s_age", "sick.s_gender|male|female", "sick_melli", "sick_s_address", "sick_s_tel",
                        // 2
                     //   "cc", "pmhx$p1$p2$p3$p4$p5$p6$p7", "other", "drug_hx", "family_hx", "other_ocular_disease",
                        // 3
                        "prev_sur_od/tags_7*", "prev_sur_os/tags_8*", "bleb_od*", "bleb_os*"];

            var table = document.getElementById("resultid");
            for (var i = 0; i < fields_id.length; i++) {
                if (i % 2 == 0) {
                    var row = table.insertRow(0);
                }
                var cell1 = row.insertCell(0);
                var cell2 = row.insertCell(1);
                cell1.innerHTML = fields_name[i];
                cell2.innerHTML = multi(fields_id[i]);//ret;//document.forms["myform"][fields_name[i]].value;
            }
            return value;
        }
        function multi(fi) {
            var ret = "";
            if (fi.indexOf("&&") == -1 && fi.indexOf("|") == -1 && fi.indexOf("$") == -1 &&
                fi.indexOf("*") == -1 && fi.indexOf("/") == -1) {
                ret = document.forms["myform"][fi].value;//fields_id[i];
            }
            else {
                ret += withDetail(fi);
                ret += withRadioBtn(fi);
                ret += withCheckbox(fi);
                ret += withDropdown(fi);
            }
            return ret;
        }

        function withDetail(fi)// to field together
        {
            var ret = "";
            if (fi.indexOf("&&") != -1) {
                var value1 = fi.split("&&");
                for (var k = 0; k < value1.length; k++) {
                    ret += document.forms["myform"][value1[k]].value + ",";
                }
                if (ret.indexOf(",") != -1)
                    ret = ret.substring(0, ret.length - 1);
            }
            return ret;
        }
        function withRadioBtn(fi)// to gender
        {
            var ret = "";
            if (fi.indexOf("|") != -1) {
                var value1 = fi.split("|");
                var v = document.forms["myform"][value1[0]].value;//0
                for (var k = 0; k < value1.length; k++) {
                    if (v == k)
                        ret = value1[k + 1];
                }
            }
            return ret;
        }
        function withCheckbox(fi)// to pmhx items
        {
            var ret = "";
            if (fi.indexOf("$") != -1) {
                var value1 = fi.split("$"); //p0 p1 p2 .. p8
                var v = document.forms["myform"][value1[0]].value;// 124
                for (var i = 0; i < value1.length; i++) {
                    if (v.indexOf(document.forms["myform"][value1[i]].value) != -1) {
                        ret += $("input[name='" + value1[i] + "']")[0].nextSibling.nodeValue + ",";
                    }
                }
            }
            return ret;
        }
        function withDropdown(fi)// to prev_sur_os items
        {
            var ret = "";
            if (fi.indexOf("/") == -1 && fi.indexOf("*") != -1) {
                var value2 = fi.substring(0, fi.indexOf("*")); // tags_8				
                ret += document.forms["myform"][value2].value + ",";
                if (ret.indexOf(",") != -1)
                    ret = ret.substring(0, ret.length - 1);
            }
            else if (fi.indexOf("/") != -1 && fi.indexOf("*") != -1) {
                var value1 = fi.substring(0, fi.indexOf("/"));  // prev_sur_os
                var value2 = fi.substring(fi.indexOf("/") + 1, fi.indexOf("*")); // tags_8
                var x = document.getElementById(value1);
                for (var i = 0; i < x.length; i++) {
                    if (x.options[i].selected)
                        ret += x.options[i].text + ",";
                }
                ret += document.forms["myform"][value2].value + ",";
                if (ret.indexOf(",") != -1)
                    ret = ret.substring(0, ret.length - 1);
                //ret += value1;
            }
            return ret;
        }















        // Your Step validation logic
        function validateSteps(stepnumber) {
            var isStepValid = true;
            switch (stepnumber) {
                case "1": isStepValid = validateStep1(); break;
                case "2": isStepValid = true;/*validateStep2();*/ break;
                case "3": isStepValid = true;/*validateStep3();*/ break;
                case "4": isStepValid = validateStep4(); break;
                case "5": isStepValid = validateStep5(); break;
                case "6": isStepValid = validateStep6(); break;
                case "7": isStepValid = validateStep7(); break;
                case "8": isStepValid = validateStep8(); break;
                case "9": isStepValid = validateStep9(); break;
                case "10": isStepValid = true; break;//validateStep11(); break;
                case "11": isStepValid = true; break;//validateStep11(); break;
            }
            return isStepValid;
        }
        function validateStep1() {
            if (document.forms["myform"]["adm"].value === "") {
                document.getElementById("msg_permission").textContent = "Please Select Patient For Exam Then Create Exam";
                return false;
            }
            else {
                document.getElementById("msg_permission").textContent = "";
            }
            var fields = ["exam_date"];
            var valfields = ["msg_exam_date"];
            return checkValidation(fields, valfields);
            return true;
        }

        //var fields = ["", "", "", "", "", "",
        //               "", "", "", "", "", ""];
        //var valfields = ["msg_", "msg_", "msg_", "msg_", "msg_", "msg_",
        //            "msg_", "msg_", "msg_", "msg_", "msg_", "msg_"];

        function validateStep2() {
            var fields = ["va_pg_od", "va_sc_od", "va_cc_od", "va_pg_os", "va_sc_os", "va_cc_os",
                        "refrection_pg_od", "refrection_sc_od", "refrection_cc_od", "refrection_pg_os", "refrection_sc_os", "refrection_cc_os"];
            var valfields = ["msg_va_pg_od", "msg_va_sc_od", "msg_va_cc_od", "msg_va_pg_os", "msg_va_sc_os", "msg_va_cc_os",
                        "msg_refrection_pg_od", "msg_refrection_sc_od", "msg_refrection_cc_od", "msg_refrection_pg_os", "msg_refrection_sc_os", "msg_refrection_cc_os"];
            return checkValidation(fields, valfields);
        }

        function validateStep3() {
            var fields = ["cc", "other", "drug_hx", "family_hx", "other_ocular_disease", "prev_sur_od",
                        "prev_sur_os"];
            var valfields = ["msg_cc", "msg_other", "msg_drug_hx", "msg_family_hx", "msg_other_ocular_disease", "msg_Previous_Surgery_OD",
                        "msg_Previous_Surgery_OS"];
            var phmxitems = ["p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8"];
            var isValid = validateCheckRadio(phmxitems, "msg_pp", "Please fill phmx");
            isValid = checkValidation(fields, valfields)
            return isValid;
        }

        function validateStep4() {
            var fields = ["lids_od", "lids_os", "alignment"];
            var valfields = ["msg_Lids_OD", "msg_Lids_OS", "msg_Alignment"];
            var isValid = checkValidation(fields, valfields)
            return isValid;
        }

        function validateStep5() {
            var fields = ["clarity_od", "clarity_os"];
            var valfields = ["msg_Clarity_OD", "msg_Clarity_OS"];
            if (document.forms["myform"]["Conjunctiva_OD"].value === "") {
                document.getElementById("msg_Conjunctiva_ODR").textContent = "Please fill Conjunctiva OD";
                return false;
            }
            if (document.forms["myform"]["Conjunctiva_OS"].value === "") {
                document.getElementById("msg_Conjunctiva_OSR").textContent = "Please fill Conjunctiva OS";
                return false;
            }
            if (document.forms["myform"]["Deep_OD"].value === "" && document.getElementById("deepod").checked === false) {
                document.getElementById("msg_Deep_OD").textContent = "Please fill one of these fields";
                return false;
            }
            if (document.forms["myform"]["Deep_OS"].value === "" && document.getElementById("deepos").checked === false) {
                document.getElementById("msg_Deep_OS").textContent = "Please fill one of these fields";
                return false;
            }
            var isValid = checkValidation(fields, valfields);
            if (!isValid)
                return false;
            return isValid;
        }

        function validateStep6() {
            var fields = ["Inferiortxtod","Inferiortxtos","Nasaltxtod","Nasaltxtos","Superiortxtod","Superiortxtos",
                    "Temporaltxtod","Temporaltxtos" ,"iris_od", "iris_os"/*, "Lens_OD", "lenstxtod", "lenstxtos", "Lens_OS"*/];
            var valfields = ["msg_Inferior_OD","msg_Inferior_OS","msg_Nasal_OD","msg_Nasal_OS","msg_Superior_OD","msg_Superior_OS",
                    "msg_Temporal_OD","msg_Temporal_OS","msg_Iris_OD", "msg_Iris_OS"/*, "msg_Lens_OD", "msg_CatNS_OD", "msg_CatNS_OS", "msg_Lens_OS"*/];
            var isValid = true;
            if (document.forms["myform"]["Lens_OD"].value === "" && document.forms["myform"]["lenstxtod"].value === "") {
                document.getElementById("msg_Lens_OD").textContent = "Please Fill One Of Fields";
                return false;
            }
            if (document.forms["myform"]["Lens_OS"].value === "" && document.forms["myform"]["lenstxtos"].value === "") {
                document.getElementById("msg_Lens_OS").textContent = "Please Fill One Of Fields";
                return false;
            }
            isValid = checkValidation(fields, valfields)
            //  var op1 = ["Narrow1", "Open1", "PAS1"];
            //  isValid = validateCheckRadio(op1, "msg_opticoptiosPP_OD", "Please Select one of options");
            if (document.getElementById("PAS1").checked && document.forms["myform"]["pastxtod"].value === "") {
                document.getElementById("msg_opticoptiosPP_OD").textContent = "Please Pas Value";
                return false;
            }
            //  var op2 = ["Narrow2", "Open2", "PAS2"];
            //  isValid = validateCheckRadio(op2, "msg_opticoptiosPP_OS", "Please Select one of options");
            if (document.getElementById("PAS2").checked && document.forms["myform"]["pastxtos"].value === "") {
                document.getElementById("msg_opticoptiosPP_OS").textContent = "Please Pas Value";
                return false;
            }
            return isValid;
        }
        
        function validateStep7() {
            var fields = [/*"Fundus_OD", "Fundus_OS",*/ "Shape_OD", "Size_OD", "Vertical_CDod",/*"Rim_notch_OD",*/
                           "Shape_OS", "Size_OS", "Vertical_CDos", "Macula_OD", "Macula_OS",// "Rim_notch_OS",
                           "Vessel_OD", "Vessel_OS"];
            var valfields = [/*"msg_Fundus_OD", "msg_Fundus_OS",*/ "msg_Shape_OD", "msg_Size_OD", "msg_Vertical_CDod_OD",//, "msg_Rim_notch_OD"
                        "msg_Shape_OS", "msg_Size_OS", "msg_Vertical_CDos_OS", "msg_Macula_OD", "msg_Macula_OS",// "msg_Rim_notch_OS",
                        "msg_Vessel_OD", "msg_Vessel_OS"];
            //var op1 = ["opticod1", "opticod2"];
            //var op2 = ["opticos1", "opticos2"];
            //var isValid = validateCheckRadio(op1, "msg_opticoptios_OD", "Please fill Optic Options OD");
            //isValid = validateCheckRadio(op2, "msg_opticoptios_OS", "Please fill Optic Options OS");
            isValid = checkValidation(fields, valfields)
            return isValid;
        }

        function validateStep8() {
            var fields = ["IOPod", "IOPos"/*, "Dorzolamideod", "Dorzolamideos", "TimololOD", "TimololOS",
                          "BrimonidineOD", "BrimonidineOS", "LatanoprostOD", "LatanoprostOS", "BetaxololOD", "BetaxololOS", "CobiosoptOD", "CobiosoptOS",
                          "Compliance_OD", "Compliance_OS"*/];
            var valfields = ["msg_IOP_OD", "msg_IOP_OS"/*, "msg_Dorzolamide_OD", "msg_Dorzolamide_OS", "msg_Timolol_OD", "msg_Timolol_OS",
                          "msg_Brimonidine_OD", "msg_Brimonidine_OS", "msg_Latanoprost_OD", "msg_Latanoprost_OS", "msg_Betaxolol_OD", "msg_Betaxolol_OS", "msg_Cobiosopt_OD", "msg_Cobiosopt_OS",
                          "msg_Compliance_OD", "msg_Compliance_OS"*/];
            var isValid = checkValidation(fields, valfields)
            return isValid;
        }

        function validateStep9() {
            /*var fields = ["non_gla_od", "non_gla_os", /*"glasus_od", "glasus_os", "gla_od", "gla_os",
                          "PlanMedicationod", "plod", "plos", "PlanMedicationos", "PlanSurgeryod", "PlanSurgeryos", "RTCdod", "RTCmod", "RTCyod",
                          "RTCdos", "RTCmos", "RTCyos"];
            var valfields = ["msg_non_gla_od", "msg_non_gla_os", /*"msg_glasus_od", "msg_glasus_os", "msg_gla_od", "msg_gla_os",
                        "msg_PlanMedication_od", "msg_pl_od", "msg_pl_os", "msg_PlanMedication_os", "msg_PlanSurgery_od", "msg_PlanSurgery_os", "msg_RTCd_od", "msg_RTCm_od", "msg_RTCy_od",
                         "msg_RTCd_os", "msg_RTCm_os", "msg_RTCy_os"];
            var isValid = checkValidation(fields, valfields);*/
            //var orparams = ["non_gla_od", "gla_od", "PlanMedicationod", "plod", "PlanSurgeryod"];
            //var isValid = checkOrVal(orparams, "msg_non_gla_od");
            //var orparams2 = ["non_gla_os", "gla_os", "PlanMedicationos", "plos", "PlanSurgeryos"];
            //isValid = checkOrVal(orparams2, "msg_non_gla_os");

            if (document.forms["myform"]["non_gla_od"].value == "" && document.forms["myform"]["gla_od"].value == ""
                     && document.forms["myform"]["glasus_od"].value == "") {
                document.getElementById("msg_glasus_od").textContent = "Please fill one of these fields";
                return false;
            }
            if (document.forms["myform"]["non_gla_os"].value == "" && document.forms["myform"]["gla_os"].value == ""
                     && document.forms["myform"]["glasus_os"].value == "") {
                document.getElementById("msg_glasus_os").textContent = "Please fill one of these fields";
                return false;
            }
            return true;
        }

        //function validateStep11() {
        //    if (document.forms["myform"]["attach_permis"].value === "y") {
        //        document.getElementById("msg_permission").textContent = "You dont Allow To Access This Tab";
        //        document.getElementById("uploadsContainer").hide();
        //        return true;
        //    }
        //    else {
        //        document.getElementById("msg_permission").textContent = "";
        //        document.getElementById("uploadsContainer").show();
        //    }
        //    return true;
        //}

        function checkOrVal(filds, varf) {
            for (var i = 0; i < filds.length; i++) {
                if (document.forms["myform"][filds[i]].value != "") {
                    return true;
                }
            }
            document.getElementById(varf).textContent = "Please Fill One Of This Part Fields";
            return false;
        }

        function checkValidation(filds, varf) {
            for (var i = 0; i < filds.length; i++) {
                if (document.forms["myform"][filds[i]].value === "") {
                    document.getElementById(varf[i]).textContent = "Please fill " + filds[i];
                    return false;
                }
                else {
                    document.getElementById(varf[i]).textContent = "";
                }
            }
            return true;
        }

        function validateCheckRadio(filds, varf, fielderrmsg) {
            var iscvalid = false;
            for (var i = 0; i < filds.length; i++) {
                if (document.getElementById(filds[i]).checked) {
                    iscvalid = true;
                }
            }
            if (!iscvalid)
                document.getElementById(varf).textContent = fielderrmsg;
            else
                document.getElementById(varf).textContent = "";
            return iscvalid;
        }
    });
