<template>
  <div class="alert-box">
    <div class="alert">
      <div class="alert__title-box">
        <h2 class="alert__title">MISA eShop</h2>
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
        <div class="alert__msg"><slot /></div>
      </div>
      <div class="alert__groupbtn-box">
        <button @click="returnValueParent(1)" class="alert__btn-yes">Có</button>
        <button @click="returnValueParent(2)" class="alert__btn-no">
          Không
        </button>
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
