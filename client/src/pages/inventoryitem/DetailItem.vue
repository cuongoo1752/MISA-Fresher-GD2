<template>
    <div>
        <!-- Group button -->
        <div class="groupbtndetail">
            <div class="groupbtndetail__btn btn-blue">
                <div class="groupbtndetail__icon detail-icon-save"></div>
                <p class="groupbtndetail__text detail-text-save">Lưu</p>
            </div>
            <div class="groupbtndetail__btn btn-white">
                <div class="groupbtndetail__icon detail-icon-dup"></div>
                <p class="groupbtndetail__text detail-text-dup">Lưu và nhân bản</p>
            </div>
            <div class="groupbtndetail__btn btn-white">
                <div class="groupbtndetail__icon detail-icon-saveadd"></div>
                <p class="groupbtndetail__text detail-text-saveadd">Lưu và thêm mới</p>
            </div>
            <div class="groupbtndetail__btn btn-outline">
                <div class="groupbtndetail__icon detail-icon-cancel"></div>
                <p class="groupbtndetail__text detail-text-cancel">Hủy bỏ</p>
            </div>
            
            
        </div>
        <div class="test">
            <BaseInput v-model="msg" :type="'number'"/>
            <BaseInputSizeAndColor :type="'color'" :id="'color'" @change="handleInputSizeOrColor"/>
            <BaseInputSizeAndColor :type="'size'" :id="'color'" @change="handleInputSizeOrColor"/>

            

        </div>
    </div>
</template>

<script>
import BaseInput from '../../components/BaseInput.vue'
import BaseInputSizeAndColor from '../../components/BaseInputSizeAndColor.vue'
    export default {
        components: {
            BaseInput,BaseInputSizeAndColor
        },
        data() {
            return {
                msg: "msg",
                isChecked: [],
                list:[]
            }
        },
        methods: {
            handleInputSizeOrColor(id, state, list, element){
                if(id == 'color'){
                    console.log(state)
                    console.log(list)
                    console.log(element)
                }
            }
        },
    }
</script>
<style scoped>
@import '../../css/pages/inventoryitem/groupbtndetail.css';
@import '../../css/components/basecheckbox.css';
@import '../../css/pages/inventoryitem/detailitem.css';
.test{
    padding: 20px;
    padding-top: 100px;
    background: white;
}
</style>