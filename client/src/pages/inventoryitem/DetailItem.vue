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

            <base-checkbox :value="isChecked" @input="isChecked"/>

        </div>
    </div>
</template>

<script>
import BaseCheckbox from '../../components/BaseCheckbox.vue'
import BaseInput from '../../components/BaseInput.vue'
    export default {
        components: {
            BaseInput,
                BaseCheckbox
        },
        data() {
            return {
                msg: "msg",
                isChecked: true,
            }
        },
        methods: {
        
        },
    }
</script>
<style scoped>
@import '../../css/pages/inventoryitem/groupbtndetail.css';
@import '../../css/pages/inventoryitem/detailitem.css';
.test{
    padding: 20px;
    padding-top: 100px;
    background: white;
}
</style>