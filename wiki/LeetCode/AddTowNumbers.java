/**
 * Definition for singly-linked list.
 * public class ListNode {
 *     int val;
 *     ListNode next;
 *     ListNode(int x) { val = x; }
 * }
 */
public class Solution {
    public ListNode addTwoNumbers(ListNode l1, ListNode l2) {
        ListNode ret = null;
        ListNode last = null;
        int fix = 0;
        while (l1 != null || l2 != null || fix > 0)
        {
            if (l1 == null) {
                l1 = new ListNode(0);
            }
            if (l2 == null) {
                l2 = new ListNode(0);
            }
            int n = l1.val + l2.val + fix;
            l1 = l1.next;
            l2 = l2.next;
            ListNode l = new ListNode(n%10);
            fix = n / 10;
            if (last == null) {
                ret = l;
            }
            else {
                last.next = l;
            }
            last = l;
        }
        return ret;
    }
}