#! /usr/bin/env bash
#
# Make bootable device

device=/dev/sdc
partnum=2
part=$device$partnum

echo "Installing to $part"

#mkfs.ext4 $part

mount $part /mnt

#force in case of "do not proceed with blocklists"
#sudo grub-install --force --root-directory=/mnt $part

#write grub.cfg
cat > /mnt/boot/grub/grub.cfg << EOF

if loadfont /boot/grub/font.pf2 ; then
set gfxmode=auto
insmod efi_gop
insmod efi_uga
insmod gfxterm
terminal_output gfxterm
fi

set menu_color_normal=white/black
set menu_color_highlight=black/light-gray

#menuentry "Try Lubuntu without installing" {
#set gfxpayload=keep
#linux/casper/vmlinuz.efi  file=/cdrom/preseed/lubuntu.seed boot=casper quiet splash ---
#initrd/casper/initrd.lz
#}

menuentry "GParted Live" {
loopback loop (hostdisk/$device,$partnum)/isos
linux (loop)/casper/vmlinuz.efi boot=casper iso-scan/filename=gparted.iso quiet splash
initrd (loop)/casper/initrd.lz
}

EOF

umount $part

echo "Done"
