#!/usr/bin/env python3
import rospy
from std_msgs.msg import Header
from sensor_msgs.msg import FluidPressure
import random

def publish_diff_pressure():
    # Initialize the ROS node
    rospy.init_node('diff_pressure_publisher', anonymous=True)

    # Create a publisher for the diff_pressure topic
    pub = rospy.Publisher('/mavros/imu/diff_pressure', FluidPressure, queue_size=10)

    # Set the loop rate in Hz
    rate = rospy.Rate(10)  # 10 Hz

    while not rospy.is_shutdown():
        # Create a FluidPressure message
        msg = FluidPressure()

        # Set the header of the message
        msg.header = Header()
        msg.header.seq = 0
        msg.header.stamp = rospy.Time.now()
        msg.header.frame_id = "pressure_sensor"

        # Set the fluid pressure value to a random value between 400 and 2000
        msg.fluid_pressure = random.uniform(500.0, 20000.0)

        # Set the variance to 0
        msg.variance = 0.0

        # Publish the message
        pub.publish(msg)

        # Sleep for the remaining time to achieve the desired loop rate
        rate.sleep()

if __name__ == '__main__':
    try:
        publish_diff_pressure()
    except rospy.ROSInterruptException:
        pass

