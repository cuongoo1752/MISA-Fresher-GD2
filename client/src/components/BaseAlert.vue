<template>
  <div class="alert-box">
    <div class="alert">
      <div class="alert__title-box">
        <h2 class="alert__title">{{title}}</h2>
        <div @click="returnValueParent(2)" class="alert__icon-cancel"></div>
      </div>
      <div class="alert__content-box">
        <div
          class="alert__icon-type "
          :class="{
            alert__info: type == 'info',
            alert__warning: type == 'warning',
            alert__danger: type == 'danger',
            alert__confuse: type == 'confuse',
          }"
        ></div>
        <div class="alert__msg"><slot></slot></div>
      </div>
      <div v-if="numberOfButton == 2" class="alert__groupbtn-box">
        <button @click="returnValueParent(1)" class="alert__btn-yes">Có</button>
        <button @click="returnValueParent(2)" class="alert__btn-no">
          Không
        </button>
      </div>
      <div v-if="numberOfButton == 1" class="alert__groupbtn-box">
        <button @click="returnValueParent(1)" class="alert__btn-yes">Đồng ý</button>
      </div>
    </div>

    <div class="hiden"></div>
  </div>
</template>

<script>
export default {
  props: {
    id: {
      type: String,
      default: "",
    },
    type: {
      type: String,
      default: "confuse",
    },
    title:{
      type: String,
      default: 'MISA eshop'
    },
    numberOfButton:{
      type: Number,
      default: 2
    }

  },
  methods: {
    returnValueParent(value) {
      this.$emit("change", this.id, value);
    },
  },
};
</script>

<style scoped>
@import "../css/components/baseinput.css";
@import "../css/components/basealert.css";
</style>
