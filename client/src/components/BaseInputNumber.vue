<template>
  <div class="baseinput" :style="{ width: width }">
    <div v-if="hasFront" class="baseinput__front">{{ textFront }}</div>
    <input

      :style="{ textAlign: textAlign }"
      :disabled="disabled"
      type="number"
      class="baseinput__input"
      :class="{ 'has-front': hasFront, 'is-input-error': isError }"
      :placeholder="placeholder"
      :value="value"
      @input="handleInput($event)"
      @blur="handleBlueInput($event)"
    />
    <div
      v-if="isError"
      v-tooltip.right-end="textVadidate"
      class="baseinput__icon-validate"
    ></div>
  </div>
</template>

<script>
import Vue from "vue";
import VTooltip from "v-tooltip";

Vue.use(VTooltip);
export default {
  props: {
    // Trạng thái
    // - text
    // - number
    // - text fielt
    type: {
      type: String,
      default: "text",
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    placeholder: {
      type: String,
      default: "",
    },
    // Vadidate
    required: {
      type: Boolean,
      default: false,
    },
    maxlength: {
      type: Number,
      default: 255,
    },
    value: {
      type: Number
    },

    hasFront: {
      type: Boolean,
      default: false,
    },
    textFront: {
      type: String,
      default: "",
    },
    width: {
      type: String,
      default: "238px",
    },

    textAlign: {
      type: String,
      default: "left",
    },
  },

  data() {
    return {
      textVadidate: "",
      isError: false,
    };
  },
  methods: {
    handleInput(event) {
      //Kiểm tra các trường
      event = event ? event : window.event;
      var charCode = event.which ? event.which : event.keyCode;
      if (this.type == "number") {
        if (
          charCode > 31 &&
          (charCode < 48 || charCode > 57) &&
          charCode !== 46
        ) {
          event.preventDefault();
        } else {
          this.$emit("input", parseInt( event.target.value));
        }
      } else if (this.type == "text") {
        this.$emit("input", parseInt( event.target.value));
      }
    },
    handleBlueInput(event) {
      if (this.required == true) {
        if (
          this.value == null ||
          this.value == undefined ||
          this.value.trim() == ""
        ) {
          this.textVadidate = "Trường này không được để trống";
          this.isError = true;
        } else {
          this.isError = false;
        }
      }

      this.$emit("blur", event);
    },
  },
};
</script>

<style scoped>
@import "../css/components/tooltip.css";
@import "../css/components/baseinput.css";
</style>
