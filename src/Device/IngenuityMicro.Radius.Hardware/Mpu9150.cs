/* ===================================================================================
 * Class to interface with InvenSense Mpu9150 sensor 
 * ===================================================================================
 * Based on work by Thomas D. Kryger http://www.tinyclr.com/codeshare/entry/383
 * and I2Cdevlib http://www.i2cdevlib.com/devices/mpu6050#links
 * Modified for MPU600 by Doug Turner
 * ===================================================================================
 * You may use, copy, borrow, modify or do anything you like with this code. Crediting
 * the authors will be appreciated.
 * ===================================================================================
 */

using System;
using Microsoft.SPOT.Hardware;

namespace IngenuityMicro.Radius.Hardware
{
    /// <summary>
    /// </summary>
    public class Mpu9150 : AbstractI2CDevice
    {
        #region constructor
        public Mpu9150(ushort address, int clockRate, int timeout, Cpu.Pin InterruptPort)
            : base(address, clockRate, timeout)
        {
            if (!Connected())
               // throw new Exception("Mpu9150 not connected");
            if (InterruptPort != Cpu.Pin.GPIO_NONE)
            {
                interruptPort = new InterruptPort(InterruptPort, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh);
            }

        }

        #endregion

        #region Delegates and events
        public delegate void Mpu9150NewData(uint data1, uint data2, DateTime time);
        public event Mpu9150NewData OnNewData;

        void interruptPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            if (OnNewData != null)
                OnNewData(data1, data2, time);
        }
        #endregion

        #region Consts
        #region Scale Factors

        #endregion
        #endregion

        #region enums
        /// <summary>
        /// Defines the types of events the device raises.
        /// </summary>

        /// <summary>
        /// The information for the Device Register is from Jeff Rowberg at i2Cdevlib
        /// published here:  http://www.i2cdevlib.com/devices/mpu6050#registers
        /// Most of the registers are not used in this implimentation of the driver
        /// </summary>
        public enum Register : byte
        {
            WHO_AM_I = 0x75,
            Mpu9150_ADDRESS_AD0_LOW = 0x68, // address pin low (GND), default for InvenSense evaluation board
            Mpu9150_ADDRESS_AD0_HIGH = 0x69, // address pin high (VCC)
            Mpu9150_DEFAULT_ADDRESS = Mpu9150_ADDRESS_AD0_LOW,

            Mpu9150_RA_XG_OFFS_TC = 0x00, // 7] PWR_MODE, [6:1] XG_OFFS_TC, [0] OTP_BNK_VLD
            Mpu9150_RA_YG_OFFS_TC = 0x01, // 7] PWR_MODE, [6:1] YG_OFFS_TC, [0] OTP_BNK_VLD
            Mpu9150_RA_ZG_OFFS_TC = 0x02, // 7] PWR_MODE, [6:1] ZG_OFFS_TC, [0] OTP_BNK_VLD
            Mpu9150_RA_X_FINE_GAIN = 0x03, // 7:0] X_FINE_GAIN
            Mpu9150_RA_Y_FINE_GAIN = 0x04, // 7:0] Y_FINE_GAIN
            Mpu9150_RA_Z_FINE_GAIN = 0x05, // 7:0] Z_FINE_GAIN
            Mpu9150_RA_XA_OFFS_H = 0x06, // 15:0] XA_OFFS
            Mpu9150_RA_XA_OFFS_L_TC = 0x07,
            Mpu9150_RA_YA_OFFS_H = 0x08, // 15:0] YA_OFFS
            Mpu9150_RA_YA_OFFS_L_TC = 0x09,
            Mpu9150_RA_ZA_OFFS_H = 0x0A, // 15:0] ZA_OFFS
            Mpu9150_RA_ZA_OFFS_L_TC = 0x0B,
            Mpu9150_RA_XG_OFFS_USRH = 0x13, // 15:0] XG_OFFS_USR
            Mpu9150_RA_XG_OFFS_USRL = 0x14,
            Mpu9150_RA_YG_OFFS_USRH = 0x15, // 15:0] YG_OFFS_USR
            Mpu9150_RA_YG_OFFS_USRL = 0x16,
            Mpu9150_RA_ZG_OFFS_USRH = 0x17, // 15:0] ZG_OFFS_USR
            Mpu9150_RA_ZG_OFFS_USRL = 0x18,
            Mpu9150_RA_SMPLRT_DIV = 0x19,
            Mpu9150_RA_CONFIG = 0x1A,
            Mpu9150_RA_GYRO_CONFIG = 0x1B,
            Mpu9150_RA_ACCEL_CONFIG = 0x1C,
            Mpu9150_RA_FF_THR = 0x1D,
            Mpu9150_RA_FF_DUR = 0x1E,
            Mpu9150_RA_MOT_THR = 0x1F,
            Mpu9150_RA_MOT_DUR = 0x20,
            Mpu9150_RA_ZRMOT_THR = 0x21,
            Mpu9150_RA_ZRMOT_DUR = 0x22,
            Mpu9150_RA_FIFO_EN = 0x23,
            Mpu9150_RA_I2C_MST_CTRL = 0x24,
            Mpu9150_RA_I2C_SLV0_ADDR = 0x25,
            Mpu9150_RA_I2C_SLV0_REG = 0x26,
            Mpu9150_RA_I2C_SLV0_CTRL = 0x27,
            Mpu9150_RA_I2C_SLV1_ADDR = 0x28,
            Mpu9150_RA_I2C_SLV1_REG = 0x29,
            Mpu9150_RA_I2C_SLV1_CTRL = 0x2A,
            Mpu9150_RA_I2C_SLV2_ADDR = 0x2B,
            Mpu9150_RA_I2C_SLV2_REG = 0x2C,
            Mpu9150_RA_I2C_SLV2_CTRL = 0x2D,
            Mpu9150_RA_I2C_SLV3_ADDR = 0x2E,
            Mpu9150_RA_I2C_SLV3_REG = 0x2F,
            Mpu9150_RA_I2C_SLV3_CTRL = 0x30,
            Mpu9150_RA_I2C_SLV4_ADDR = 0x31,
            Mpu9150_RA_I2C_SLV4_REG = 0x32,
            Mpu9150_RA_I2C_SLV4_DO = 0x33,
            Mpu9150_RA_I2C_SLV4_CTRL = 0x34,
            Mpu9150_RA_I2C_SLV4_DI = 0x35,
            Mpu9150_RA_I2C_MST_STATUS = 0x36,

            Mpu9150_RA_INT_PIN_CFG = 0x37,
            Mpu9150_RA_INT_ENABLE = 0x38,
            Mpu9150_RA_DMP_INT_STATUS = 0x39,
            Mpu9150_RA_INT_STATUS = 0x3A,

            Mpu9150_RA_ACCEL_XOUT_H = 0x3B,
            Mpu9150_RA_ACCEL_XOUT_L = 0x3C,
            Mpu9150_RA_ACCEL_YOUT_H = 0x3D,
            Mpu9150_RA_ACCEL_YOUT_L = 0x3E,
            Mpu9150_RA_ACCEL_ZOUT_H = 0x3F,
            Mpu9150_RA_ACCEL_ZOUT_L = 0x40,
            Mpu9150_RA_TEMP_OUT_H = 0x41,
            Mpu9150_RA_TEMP_OUT_L = 0x42,
            Mpu9150_RA_GYRO_XOUT_H = 0x43,
            Mpu9150_RA_GYRO_XOUT_L = 0x44,
            Mpu9150_RA_GYRO_YOUT_H = 0x45,
            Mpu9150_RA_GYRO_YOUT_L = 0x46,
            Mpu9150_RA_GYRO_ZOUT_H = 0x47,
            Mpu9150_RA_GYRO_ZOUT_L = 0x48,

            Mpu9150_RA_EXT_SENS_DATA_00 = 0x49,
            Mpu9150_RA_EXT_SENS_DATA_01 = 0x4A,
            Mpu9150_RA_EXT_SENS_DATA_02 = 0x4B,
            Mpu9150_RA_EXT_SENS_DATA_03 = 0x4C,
            Mpu9150_RA_EXT_SENS_DATA_04 = 0x4D,
            Mpu9150_RA_EXT_SENS_DATA_05 = 0x4E,
            Mpu9150_RA_EXT_SENS_DATA_06 = 0x4F,
            Mpu9150_RA_EXT_SENS_DATA_07 = 0x50,
            Mpu9150_RA_EXT_SENS_DATA_08 = 0x51,
            Mpu9150_RA_EXT_SENS_DATA_09 = 0x52,
            Mpu9150_RA_EXT_SENS_DATA_10 = 0x53,
            Mpu9150_RA_EXT_SENS_DATA_11 = 0x54,
            Mpu9150_RA_EXT_SENS_DATA_12 = 0x55,
            Mpu9150_RA_EXT_SENS_DATA_13 = 0x56,
            Mpu9150_RA_EXT_SENS_DATA_14 = 0x57,
            Mpu9150_RA_EXT_SENS_DATA_15 = 0x58,
            Mpu9150_RA_EXT_SENS_DATA_16 = 0x59,
            Mpu9150_RA_EXT_SENS_DATA_17 = 0x5A,
            Mpu9150_RA_EXT_SENS_DATA_18 = 0x5B,
            Mpu9150_RA_EXT_SENS_DATA_19 = 0x5C,
            Mpu9150_RA_EXT_SENS_DATA_20 = 0x5D,
            Mpu9150_RA_EXT_SENS_DATA_21 = 0x5E,
            Mpu9150_RA_EXT_SENS_DATA_22 = 0x5F,
            Mpu9150_RA_EXT_SENS_DATA_23 = 0x60,
            Mpu9150_RA_MOT_DETECT_STATUS = 0x61,
            Mpu9150_RA_I2C_SLV0_DO = 0x63,
            Mpu9150_RA_I2C_SLV1_DO = 0x64,
            Mpu9150_RA_I2C_SLV2_DO = 0x65,
            Mpu9150_RA_I2C_SLV3_DO = 0x66,
            Mpu9150_RA_I2C_MST_DELAY_CTRL = 0x67,
            Mpu9150_RA_SIGNAL_PATH_RESET = 0x68,
            Mpu9150_RA_MOT_DETECT_CTRL = 0x69,
            Mpu9150_RA_USER_CTRL = 0x6A,

            Mpu9150_RA_PWR_MGMT_1 = 0x6B,
            Mpu9150_RA_PWR_MGMT_2 = 0x6C,

            Mpu9150_RA_BANK_SEL = 0x6D,
            Mpu9150_RA_MEM_START_ADDR = 0x6E,
            Mpu9150_RA_MEM_R_W = 0x6F,
            Mpu9150_RA_DMP_CFG_1 = 0x70,
            Mpu9150_RA_DMP_CFG_2 = 0x71,
            Mpu9150_RA_FIFO_COUNTH = 0x72,
            Mpu9150_RA_FIFO_COUNTL = 0x73,
            Mpu9150_RA_FIFO_R_W = 0x74,
            Mpu9150_RA_WHO_AM_I = 0x75,

            Mpu9150_TC_PWR_MODE_BIT = 7,
            Mpu9150_TC_OFFSET_BIT = 6,
            Mpu9150_TC_OFFSET_LENGTH = 6,
            Mpu9150_TC_OTP_BNK_VLD_BIT = 0,

            Mpu9150_VDDIO_LEVEL_VLOGIC = 0,
            Mpu9150_VDDIO_LEVEL_VDD = 1,

            Mpu9150_CFG_EXT_SYNC_SET_BIT = 5,
            Mpu9150_CFG_EXT_SYNC_SET_LENGTH = 3,
            Mpu9150_CFG_DLPF_CFG_BIT = 2,
            Mpu9150_CFG_DLPF_CFG_LENGTH = 3,

            Mpu9150_EXT_SYNC_DISABLED = 0x0,
            Mpu9150_EXT_SYNC_TEMP_OUT_L = 0x1,
            Mpu9150_EXT_SYNC_GYRO_XOUT_L = 0x2,
            Mpu9150_EXT_SYNC_GYRO_YOUT_L = 0x3,
            Mpu9150_EXT_SYNC_GYRO_ZOUT_L = 0x4,
            Mpu9150_EXT_SYNC_ACCEL_XOUT_L = 0x5,
            Mpu9150_EXT_SYNC_ACCEL_YOUT_L = 0x6,
            Mpu9150_EXT_SYNC_ACCEL_ZOUT_L = 0x7,

            Mpu9150_DLPF_BW_256 = 0x00,
            Mpu9150_DLPF_BW_188 = 0x01,
            Mpu9150_DLPF_BW_98 = 0x02,
            Mpu9150_DLPF_BW_42 = 0x03,
            Mpu9150_DLPF_BW_20 = 0x04,
            Mpu9150_DLPF_BW_10 = 0x05,
            Mpu9150_DLPF_BW_5 = 0x06,

            Mpu9150_GCONFIG_FS_SEL_BIT = 4,
            Mpu9150_GCONFIG_FS_SEL_LENGTH = 2,

            Mpu9150_GYRO_FS_250 = 0x00,
            Mpu9150_GYRO_FS_500 = 0x01,
            Mpu9150_GYRO_FS_1000 = 0x02,
            Mpu9150_GYRO_FS_2000 = 0x03,

            Mpu9150_ACONFIG_XA_ST_BIT = 7,
            Mpu9150_ACONFIG_YA_ST_BIT = 6,
            Mpu9150_ACONFIG_ZA_ST_BIT = 5,
            Mpu9150_ACONFIG_AFS_SEL_BIT = 4,
            Mpu9150_ACONFIG_AFS_SEL_LENGTH = 2,
            Mpu9150_ACONFIG_ACCEL_HPF_BIT = 2,
            Mpu9150_ACONFIG_ACCEL_HPF_LENGTH = 3,

            Mpu9150_ACCEL_FS_2 = 0x00,
            Mpu9150_ACCEL_FS_4 = 0x01,
            Mpu9150_ACCEL_FS_8 = 0x02,
            Mpu9150_ACCEL_FS_16 = 0x03,

            PU6050_DHPF_RESET = 0x00,
            Mpu9150_DHPF_5 = 0x01,
            Mpu9150_DHPF_2P5 = 0x02,
            Mpu9150_DHPF_1P25 = 0x03,
            Mpu9150_DHPF_0P63 = 0x04,
            Mpu9150_DHPF_HOLD = 0x07,

            Mpu9150_TEMP_FIFO_EN_BIT = 7,
            Mpu9150_XG_FIFO_EN_BIT = 6,
            Mpu9150_YG_FIFO_EN_BIT = 5,
            Mpu9150_ZG_FIFO_EN_BIT = 4,
            Mpu9150_ACCEL_FIFO_EN_BIT = 3,
            Mpu9150_SLV2_FIFO_EN_BIT = 2,
            Mpu9150_SLV1_FIFO_EN_BIT = 1,
            Mpu9150_SLV0_FIFO_EN_BIT = 0,

            Mpu9150_MULT_MST_EN_BIT = 7,
            Mpu9150_WAIT_FOR_ES_BIT = 6,
            Mpu9150_SLV_3_FIFO_EN_BIT = 5,
            Mpu9150_I2C_MST_P_NSR_BIT = 4,
            Mpu9150_I2C_MST_CLK_BIT = 3,
            Mpu9150_I2C_MST_CLK_LENGTH = 4,

            Mpu9150_CLOCK_DIV_348 = 0x0,
            Mpu9150_CLOCK_DIV_333 = 0x1,
            Mpu9150_CLOCK_DIV_320 = 0x2,
            Mpu9150_CLOCK_DIV_308 = 0x3,
            Mpu9150_CLOCK_DIV_296 = 0x4,
            Mpu9150_CLOCK_DIV_286 = 0x5,
            Mpu9150_CLOCK_DIV_276 = 0x6,
            Mpu9150_CLOCK_DIV_267 = 0x7,
            Mpu9150_CLOCK_DIV_258 = 0x8,
            Mpu9150_CLOCK_DIV_500 = 0x9,
            Mpu9150_CLOCK_DIV_471 = 0xA,
            Mpu9150_CLOCK_DIV_444 = 0xB,
            Mpu9150_CLOCK_DIV_421 = 0xC,
            Mpu9150_CLOCK_DIV_400 = 0xD,
            Mpu9150_CLOCK_DIV_381 = 0xE,
            Mpu9150_CLOCK_DIV_364 = 0xF,

            Mpu9150_I2C_SLV_RW_BIT = 7,
            Mpu9150_I2C_SLV_ADDR_BIT = 6,
            Mpu9150_I2C_SLV_ADDR_LENGTH = 7,
            Mpu9150_I2C_SLV_EN_BIT = 7,
            Mpu9150_I2C_SLV_BYTE_SW_BIT = 6,
            Mpu9150_I2C_SLV_REG_DIS_BIT = 5,
            Mpu9150_I2C_SLV_GRP_BIT = 4,
            Mpu9150_I2C_SLV_LEN_BIT = 3,
            Mpu9150_I2C_SLV_LEN_LENGTH = 4,

            Mpu9150_I2C_SLV4_RW_BIT = 7,
            Mpu9150_I2C_SLV4_ADDR_BIT = 6,
            Mpu9150_I2C_SLV4_ADDR_LENGTH = 7,
            Mpu9150_I2C_SLV4_EN_BIT = 7,
            Mpu9150_I2C_SLV4_INT_EN_BIT = 6,
            Mpu9150_I2C_SLV4_REG_DIS_BIT = 5,
            Mpu9150_I2C_SLV4_MST_DLY_BIT = 4,
            Mpu9150_I2C_SLV4_MST_DLY_LENGTH = 5,

            Mpu9150_MST_PASS_THROUGH_BIT = 7,
            Mpu9150_MST_I2C_SLV4_DONE_BIT = 6,
            Mpu9150_MST_I2C_LOST_ARB_BIT = 5,
            Mpu9150_MST_I2C_SLV4_NACK_BIT = 4,
            Mpu9150_MST_I2C_SLV3_NACK_BIT = 3,
            Mpu9150_MST_I2C_SLV2_NACK_BIT = 2,
            Mpu9150_MST_I2C_SLV1_NACK_BIT = 1,
            Mpu9150_MST_I2C_SLV0_NACK_BIT = 0,

            Mpu9150_INTCFG_INT_LEVEL_BIT = 7,
            Mpu9150_INTCFG_INT_OPEN_BIT = 6,
            Mpu9150_INTCFG_LATCH_INT_EN_BIT = 5,
            Mpu9150_INTCFG_INT_RD_CLEAR_BIT = 4,
            Mpu9150_INTCFG_FSYNC_INT_LEVEL_BIT = 3,
            Mpu9150_INTCFG_FSYNC_INT_EN_BIT = 2,
            Mpu9150_INTCFG_I2C_BYPASS_EN_BIT = 1,
            Mpu9150_INTCFG_CLKOUT_EN_BIT = 0,

            Mpu9150_INTMODE_ACTIVEHIGH = 0x00,
            Mpu9150_INTMODE_ACTIVELOW = 0x01,

            Mpu9150_INTDRV_PUSHPULL = 0x00,
            Mpu9150_INTDRV_OPENDRAIN = 0x01,

            Mpu9150_INTLATCH_50USPULSE = 0x00,
            Mpu9150_INTLATCH_WAITCLEAR = 0x01,

            Mpu9150_INTCLEAR_STATUSREAD = 0x00,
            Mpu9150_INTCLEAR_ANYREAD = 0x01,

            Mpu9150_INTERRUPT_FF_BIT = 7,
            Mpu9150_INTERRUPT_MOT_BIT = 6,
            Mpu9150_INTERRUPT_ZMOT_BIT = 5,
            Mpu9150_INTERRUPT_FIFO_OFLOW_BIT = 4,
            Mpu9150_INTERRUPT_I2C_MST_INT_BIT = 3,
            Mpu9150_INTERRUPT_PLL_RDY_INT_BIT = 2,
            Mpu9150_INTERRUPT_DMP_INT_BIT = 1,
            Mpu9150_INTERRUPT_DATA_RDY_BIT = 0,

            // TODO: figure out what these actually do
            // UMPL source code is not very obivous
            Mpu9150_DMPINT_5_BIT = 5,
            Mpu9150_DMPINT_4_BIT = 4,
            Mpu9150_DMPINT_3_BIT = 3,
            Mpu9150_DMPINT_2_BIT = 2,
            Mpu9150_DMPINT_1_BIT = 1,
            Mpu9150_DMPINT_0_BIT = 0,

            Mpu9150_MOTION_MOT_XNEG_BIT = 7,
            Mpu9150_MOTION_MOT_XPOS_BIT = 6,
            Mpu9150_MOTION_MOT_YNEG_BIT = 5,
            Mpu9150_MOTION_MOT_YPOS_BIT = 4,
            Mpu9150_MOTION_MOT_ZNEG_BIT = 3,
            Mpu9150_MOTION_MOT_ZPOS_BIT = 2,
            Mpu9150_MOTION_MOT_ZRMOT_BIT = 0,

            Mpu9150_DELAYCTRL_DELAY_ES_SHADOW_BIT = 7,
            Mpu9150_DELAYCTRL_I2C_SLV4_DLY_EN_BIT = 4,
            Mpu9150_DELAYCTRL_I2C_SLV3_DLY_EN_BIT = 3,
            Mpu9150_DELAYCTRL_I2C_SLV2_DLY_EN_BIT = 2,
            Mpu9150_DELAYCTRL_I2C_SLV1_DLY_EN_BIT = 1,
            Mpu9150_DELAYCTRL_I2C_SLV0_DLY_EN_BIT = 0,

            Mpu9150_PATHRESET_GYRO_RESET_BIT = 2,
            Mpu9150_PATHRESET_ACCEL_RESET_BIT = 1,
            Mpu9150_PATHRESET_TEMP_RESET_BIT = 0,

            Mpu9150_DETECT_ACCEL_ON_DELAY_BIT = 5,
            Mpu9150_DETECT_ACCEL_ON_DELAY_LENGTH = 2,
            Mpu9150_DETECT_FF_COUNT_BIT = 3,
            Mpu9150_DETECT_FF_COUNT_LENGTH = 2,
            Mpu9150_DETECT_MOT_COUNT_BIT = 1,
            Mpu9150_DETECT_MOT_COUNT_LENGTH = 2,

            Mpu9150_DETECT_DECREMENT_RESET = 0x0,
            Mpu9150_DETECT_DECREMENT_1 = 0x1,
            Mpu9150_DETECT_DECREMENT_2 = 0x2,
            Mpu9150_DETECT_DECREMENT_4 = 0x3,

            Mpu9150_USERCTRL_DMP_EN_BIT = 7,
            Mpu9150_USERCTRL_FIFO_EN_BIT = 6,
            Mpu9150_USERCTRL_I2C_MST_EN_BIT = 5,
            Mpu9150_USERCTRL_I2C_IF_DIS_BIT = 4,
            Mpu9150_USERCTRL_DMP_RESET_BIT = 3,
            Mpu9150_USERCTRL_FIFO_RESET_BIT = 2,
            Mpu9150_USERCTRL_I2C_MST_RESET_BIT = 1,
            Mpu9150_USERCTRL_SIG_COND_RESET_BIT = 0,

            Mpu9150_PWR1_DEVICE_RESET_BIT = 7,
            Mpu9150_PWR1_SLEEP_BIT = 6,
            Mpu9150_PWR1_CYCLE_BIT = 5,
            Mpu9150_PWR1_TEMP_DIS_BIT = 3,
            Mpu9150_PWR1_CLKSEL_BIT = 2,
            Mpu9150_PWR1_CLKSEL_LENGTH = 3,

            Mpu9150_CLOCK_INTERNAL = 0x00,
            Mpu9150_CLOCK_PLL_XGYRO = 0x01,
            Mpu9150_CLOCK_PLL_YGYRO = 0x02,
            Mpu9150_CLOCK_PLL_ZGYRO = 0x03,
            Mpu9150_CLOCK_PLL_EXT32K = 0x04,
            Mpu9150_CLOCK_PLL_EXT19M = 0x05,
            Mpu9150_CLOCK_KEEP_RESET = 0x07,

            Mpu9150_PWR2_LP_WAKE_CTRL_BIT = 7,
            Mpu9150_PWR2_LP_WAKE_CTRL_LENGTH = 2,
            Mpu9150_PWR2_STBY_XA_BIT = 5,
            Mpu9150_PWR2_STBY_YA_BIT = 4,
            Mpu9150_PWR2_STBY_ZA_BIT = 3,
            Mpu9150_PWR2_STBY_XG_BIT = 2,
            Mpu9150_PWR2_STBY_YG_BIT = 1,
            Mpu9150_PWR2_STBY_ZG_BIT = 0,

            Mpu9150_WAKE_FREQ_1P25 = 0x0,
            Mpu9150_WAKE_FREQ_2P5 = 0x1,
            Mpu9150_WAKE_FREQ_5 = 0x2,
            Mpu9150_WAKE_FREQ_10 = 0x3,

            Mpu9150_BANKSEL_PRFTCH_EN_BIT = 6,
            Mpu9150_BANKSEL_CFG_USER_BANK_BIT = 5,
            Mpu9150_BANKSEL_MEM_SEL_BIT = 4,
            Mpu9150_BANKSEL_MEM_SEL_LENGTH = 5,

            Mpu9150_WHO_AM_I_BIT = 6,
            Mpu9150_WHO_AM_I_LENGTH = 6,

            Mpu9150_DMP_MEMORY_BANKS = 8,
            Mpu9150_DMP_MEMORY_BANK_SIZE = 254,
            //Mpu9150_DMP_MEMORY_BANK_SIZE = 256,

            Mpu9150_DMP_MEMORY_CHUNK_SIZE = 16,



        }


        #region Sensor Scale and Offsets

        //Scaling factor -
        double AccelScaleFactor = 16384; //Returns G's i.e 1G when vertical


        //* FS_SEL | Full Scale Range   | LSB Sensitivity
        //* -------+--------------------+----------------
        //* 0      | +/- 250 degrees/s  | 131 LSB/deg/s
        //* 1      | +/- 500 degrees/s  | 65.5 LSB/deg/s
        //* 2      | +/- 1000 degrees/s | 32.8 LSB/deg/s
        //* 3      | +/- 2000 degrees/s | 16.4 LSB/deg/s


        double GyroScaleFactor = 131;

        //The offset for each Mpu9150, updated when the calibration function is called
        //These values have been found by empirical testing on my 
        double xGyroOffset = -0.852;
        double yGyroOffset = 1.39;
        double zGyroOffset = -1.52;



        //The offset for each Mpu9150, updated when the calibration function is called
        double xAccelOffset = 0;
        double yAccelOffset = 0;
        double zAccelOffset = 0;
        #endregion


        #endregion

        #region fields
        private InterruptPort interruptPort;
        #endregion

        #region Properties

        public double aX { get { return ReadShort((byte)Register.Mpu9150_RA_ACCEL_XOUT_H, (byte)Register.Mpu9150_RA_ACCEL_XOUT_L); } }

        /// <summary>
        /// Returns the Y axis reading.
        /// </summary>
        public double aY { get { return ReadShort((byte)Register.Mpu9150_RA_ACCEL_YOUT_H, (byte)Register.Mpu9150_RA_ACCEL_YOUT_L); } }

        /// <summary>
        /// Returns the Z axis reading.
        /// </summary>
        public double aZ { get { return ReadShort((byte)Register.Mpu9150_RA_ACCEL_ZOUT_H, (byte)Register.Mpu9150_RA_ACCEL_ZOUT_L); } }


        public double gX { get { return ReadShort((byte)Register.Mpu9150_RA_GYRO_XOUT_H, (byte)Register.Mpu9150_RA_GYRO_XOUT_L); } }

        /// <summary>
        /// Returns the Y axis reading.
        /// </summary>
        public double gY { get { return ReadShort((byte)Register.Mpu9150_RA_GYRO_YOUT_H, (byte)Register.Mpu9150_RA_GYRO_YOUT_L); } }

        /// <summary>
        /// Returns the Z axis reading.
        /// </summary>
        public double gZ { get { return ReadShort((byte)Register.Mpu9150_RA_GYRO_ZOUT_H, (byte)Register.Mpu9150_RA_GYRO_ZOUT_L); } }


        public byte[] IMUData { get { return Read((byte)Register.Mpu9150_RA_ACCEL_XOUT_H, 14); } } //Returns all MyMpu9150 data registreies
            
        #endregion

        #region Mpu9150 Setup

        /// <summary>
        /// Get the Address of the Gyro
        /// </summary>
        /// <returns>I2C address</returns>
        /// 




        #region Device Identity
        public override bool Connected()
        {
            byte me = DeviceIdentifier()[0];
            return (me == 0x68);
        }

        public override byte[] DeviceIdentifier()
        {
            //return new byte[1] { Read((byte)Register.DeviceId)[0] };
            return new byte[1] { Read((byte)Register.WHO_AM_I)[0] };
        }
 
        public byte getID()
        {
            //return Read((byte)Register.WHO_AM_I);
            return  Read((byte)Register.WHO_AM_I)[0] ;
        }
        #endregion


        /** Get full-scale gyroscope range.
        * The FS_SEL parameter allows setting the full-scale range of the gyro sensors,
        * as described in the table below.
        *
        * 0 = +/- 250 degrees/sec
        * 1 = +/- 500 degrees/sec
        * 2 = +/- 1000 degrees/sec
        * 3 = +/- 2000 degrees/sec
        *
        */
  
        public int getFullScaleGyroRange()
        {
            return Read((byte)Register.Mpu9150_RA_GYRO_CONFIG)[0];
        }
  
        public void setFullScaleGyroRange(byte mode)
        {
            Write((byte)Register.Mpu9150_RA_GYRO_CONFIG, mode);
        }

        /*Get full-scale gyroscope range.
        AFS_SEL selects the full scale range of the accelerometer outputs according to the following table.

        AFS_SEL Full Scale Range
        0 ± 2g
        1 ± 4g
        2 ± 8g
        3 ± 16g
        */
        public int getFullScaleAccelRange()
        {
            return Read((byte)Register.Mpu9150_RA_ACCEL_CONFIG)[0];
        }
  
        public void setFullScaleAccelRange(byte mode)
        {

            Write((byte)Register.Mpu9150_RA_ACCEL_CONFIG, mode);
        }

        public void setSleepEnabled(bool enabled)
        {
            Write((byte)Register.Mpu9150_RA_PWR_MGMT_1, 0);

        }
  
        public void Wake()
        {
            Write((byte)Register.Mpu9150_RA_PWR_MGMT_1, 1); //2g fullscale
        }

        /** Get digital low-pass filter configuration.
        * 
        * A low-pass filter eliminates frequencies above the 
        * cutoff frequency while passing those below unchanged.
        * 
        * The DLPF_CFG parameter sets the digital low pass filter configuration. It
        * also determines the internal sampling rate used by the device as shown in
        * the table below.
        *
        * Note: The accelerometer output rate is 1kHz. This means that for a Sample
        * Rate greater than 1kHz, the same accelerometer sample may be output to the
        * FIFO, DMP, and sensor registers more than once.
        *
        * <pre>
        *          |   ACCELEROMETER    |           GYROSCOPE
        * DLPF_CFG | Bandwidth | Delay  | Bandwidth | Delay  | Sample Rate
        * ---------+-----------+--------+-----------+--------+-------------
        * 0        | 260Hz     | 0ms    | 256Hz     | 0.98ms | 8kHz
        * 1        | 184Hz     | 2.0ms  | 188Hz     | 1.9ms  | 1kHz
        * 2        | 94Hz      | 3.0ms  | 98Hz      | 2.8ms  | 1kHz
        * 3        | 44Hz      | 4.9ms  | 42Hz      | 4.8ms  | 1kHz
        * 4        | 21Hz      | 8.5ms  | 20Hz      | 8.3ms  | 1kHz
        * 5        | 10Hz      | 13.8ms | 10Hz      | 13.4ms | 1kHz
        * 6        | 5Hz       | 19.0ms | 5Hz       | 18.6ms | 1kHz
        * 7        |   -- Reserved --   |   -- Reserved --   | Reserved
        * </pre>
        *
        * @return DLFP configuration
        * @see MPU6050_RA_CONFIG
        * @see MPU6050_CFG_DLPF_CFG_BIT
        * @see MPU6050_CFG_DLPF_CFG_LENGTH
        */
        public byte getDLPFMode()
        {
            return  Read((byte)Register.Mpu9150_RA_CONFIG)[0];
        }

        /** Set digital low-pass filter configuration.
         * @param mode New DLFP configuration setting
         * @see getDLPFBandwidth()
         * @see MPU6050_DLPF_BW_256
         * @see MPU6050_RA_CONFIG
         * @see MPU6050_CFG_DLPF_CFG_BIT
         * @see MPU6050_CFG_DLPF_CFG_LENGTH
         * * 
         * A low-pass filter eliminates frequencies above the 
         * cutoff frequency while passing those below unchanged.
         * 
         */
        void setDLPFMode(byte mode)
        {
            Write((byte)Register.Mpu9150_RA_CONFIG, mode);
        }

        #endregion

        #region Read Data

        /// <summary>
        /// Read the X Gyro (horizontal, parallel to text on device)
        /// </summary>
        /// <returns>degrees/sec</returns>
        public double readGyroX()
        {
               
            int data = ReadShort((byte)Register.Mpu9150_RA_GYRO_XOUT_H, (byte)Register.Mpu9150_RA_GYRO_XOUT_L);
            return (((double)(data)) / GyroScaleFactor) - xGyroOffset;
        }
        /// <summary>
        /// Read the Y Gyro (horizontal, perpendicular to text on device)
        /// </summary>
        /// <returns>degrees/sec</returns>
        /// 
        public double readGyroY()
        {
            int data = ReadShort((byte)Register.Mpu9150_RA_GYRO_YOUT_H, (byte)Register.Mpu9150_RA_GYRO_YOUT_L);
            return (((double)(data)) / GyroScaleFactor) - yGyroOffset;
        }
        /// <summary>
        /// Read the Z Gyro (vertical, looking down)
        /// </summary>
        /// <returns>degrees/sec</returns>
        /// 
        public double readGyroZ()
        {
              
            int data = ReadShort((byte)Register.Mpu9150_RA_GYRO_ZOUT_H, (byte)Register.Mpu9150_RA_GYRO_ZOUT_L);
            return ((((double)(data)) / GyroScaleFactor) - zGyroOffset);
        }

  
        public double readAccelX()
        {
            int  data = ReadShort((byte)Register.Mpu9150_RA_ACCEL_XOUT_H, (byte)Register.Mpu9150_RA_ACCEL_XOUT_L);

            return ((((double)(data)) / AccelScaleFactor) - xAccelOffset);
        }
        public double readAccelY()
        {
            int data = ReadShort((byte)Register.Mpu9150_RA_ACCEL_YOUT_H, (byte)Register.Mpu9150_RA_ACCEL_YOUT_L);

            return ((((double)(data)) / AccelScaleFactor) - yAccelOffset);
        }
        public double readAccelZ()
        {
            int data = ReadShort((byte)Register.Mpu9150_RA_ACCEL_ZOUT_H, (byte)Register.Mpu9150_RA_ACCEL_ZOUT_L);

            return (((double)(data)) / AccelScaleFactor) - zAccelOffset;
        }


        /// <summary>
        /// Read the temperature sensor
        /// </summary>
        /// <returns>Current temperature, in degrees C</returns>
        public double readTemp()
        {
            double data = ReadShort((byte)Register.Mpu9150_RA_TEMP_OUT_H, (byte)Register.Mpu9150_RA_TEMP_OUT_L);

            int Offset = 9860;//Seems to give about the right value by my empirical testing
            //340LSB/DegreeC
            data = (data + Offset) / 340;
            return  data;

        }

        //////////////////////////////////////
        /** Get raw 6-axis motion sensor readings (accel/gyro).
        * Retrieves all currently available motion sensor values.
        * @param ax 16-bit signed integer container for accelerometer X-axis value
        * @param ay 16-bit signed integer container for accelerometer Y-axis value
        * @param az 16-bit signed integer container for accelerometer Z-axis value
        * @param gx 16-bit signed integer container for gyroscope X-axis value
        * @param gy 16-bit signed integer container for gyroscope Y-axis value
        * @param gz 16-bit signed integer container for gyroscope Z-axis value
        * @see getAcceleration()
        * @see getRotation()
        * @see MPU6050_RA_ACCEL_XOUT_H
        */
        public double ax = 0;
        public double ay = 0;
        public double az = 0;
        public double gx = 0;
        public double gy = 0;
        public double gz = 0;
        public void getMotion6()
        {
            byte[] buffer;
            buffer = Read((byte)Register.Mpu9150_RA_ACCEL_XOUT_H, 14);
            ax = ((Int16)((((byte)buffer[0]) << 8) | buffer[1]) / AccelScaleFactor) - zAccelOffset;
            ay = ((Int16)((((byte)buffer[2]) << 8) | buffer[3]) / AccelScaleFactor) - zAccelOffset;
            az = ((Int16)((((byte)buffer[4]) << 8) | buffer[5]) / AccelScaleFactor) - zAccelOffset;
            gx = ((Int16)((((byte)buffer[8]) << 8) | buffer[9]) / GyroScaleFactor) - xGyroOffset;
            gy = ((Int16)((((byte)buffer[10]) << 8) | buffer[11]) / GyroScaleFactor) - yGyroOffset;
            gz = ((Int16)((((byte)buffer[12]) << 8) | buffer[13]) / GyroScaleFactor) - zGyroOffset;

        }





        #endregion

        #region private methods
          /// <summary>
          /// Converts a byte array into a short. Taken from the BitConverter in the community.
          /// </summary>
          /// <param name="value"></param>
          /// <param name="index">JCarter</param>
          /// <returns></returns>
          private short ToInt16(byte[] value, int index = 0)
          {
              return (short)(
                  value[0 + index] << 0 |
                  value[1 + index] << 8);
          }

          private double FixOneToMinus1(double d)
          {
              if (d > 1)
                  d = 1;
              else
                  if (d < -1)
                      d = -1;
              return d;
          }
          #endregion

    }
}
