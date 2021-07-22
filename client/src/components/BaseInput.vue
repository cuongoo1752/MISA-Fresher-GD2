<template>
  <div class="baseinput" :style="{ width: width }" @focus="focusInput">
    <div v-if="hasFront" class="baseinput__front">{{ textFront }}</div>
    <input
      ref="input"
      :style="{ textAlign: textAlign }"
      :disabled="disabled"
      type="text"
      class="baseinput__input"
      :class="{ 'has-front': hasFront, 'is-input-error': isError }"
      :placeholder="placeholder"
      :value="value"
      @input.stop="handleInput($event)"
      @blur.stop="handleBlueInput($event)"
    />
    <div
      tabindex="10"
      id="dangerinput"
      ref="danger"
      v-show="isError"
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
      type: String,
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
    watchState: {
      type: Number,
      default: 0,
    },
  },
  watch: {
    watchState() {},
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
          this.$emit("input", event.target.value);
        }
      } else if (this.type == "text") {
        this.$emit("input", event.target.value);
      }
      this.$emit("input", event.target.value);
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
    focusInput() {
      this.$refs.input.focus();
      let refs = this.$refs;
      if (this.required) {
        this.textVadidate = "Trường này không được để trống";
        this.isError = true;
      }

      setTimeout(function() {
        document.getElementById("dangerinput").focus();
      }, 30);
      setTimeout(function() {
        refs.input.focus();
      }, 600);
    },
  },
};
</script>

<style scoped>
@import "../css/components/tooltip.css";
@import "../css/components/baseinput.css";
</style>
